using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TimMovie.Core.Const;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.Entities;
using TimMovie.Core.Interfaces;
using TimMovie.Core.Services.ChatTemplatedNotifications;
using TimMovie.Core.Services.Messages;
using TimMovie.Core.SupportChat;
using TimMovie.SharedKernel.Extensions;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Hubs;

public class ChatHub : Hub
{
    private readonly IUserService _userService;
    private readonly MessageService _messageService;
    private readonly UserManager<User> _userManager;
    private readonly ChatTemplatedNotificationService _templatedNotificationService;

    public ChatHub(
        UserManager<User> userManager,
        IUserService userService,
        ChatTemplatedNotificationService templatedNotificationService,
        MessageService messageService)
    {
        _userManager = userManager;
        _userService = userService;
        _templatedNotificationService = templatedNotificationService;
        _messageService = messageService;
    }

    public async Task SendMessageToUser(string content)
    {
        var support = await _userManager.GetUserAsync(Context.User);
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];

        var messageDto = await _messageService.CreateNewMessageToUserAsync(new NewMessageDto
        {
           Content = content,
           Sender = support,
           GroupName = groupName
        });

        await Clients.Group(groupName).SendAsync("ReceiveNewMessage", messageDto);
    }

    public async Task SendMessageToSupport(string content)
    {
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        if (StoresForSupport.DisconnectedGroups.Contains(groupName))
        {
            if (!await TryLinkWithFreeSupport(groupName))
            {
                StoresForSupport.WaitingGroupsWithUser.Enqueue(groupName);
            }

            StoresForSupport.DisconnectedGroups.Remove(groupName);
        }
        
        var user = Context.User.Identity.IsAuthenticated 
            ? await _userManager.GetUserAsync(Context.User) 
            : null;

        var messageDto = await _messageService.CreateNewMessageToSupportAsync(new NewMessageDto
        {
           Content = content,
           Sender = user,
           GroupName = groupName
        });
        
        await Clients.Group(groupName).SendAsync("ReceiveNewMessage", messageDto);
    }

    public async Task ConnectSupport(bool isFirstStart)
    {
        var supportId = Context.User.GetUserId().Value;
        
        if (!isFirstStart)
        {
            await Clients.User(supportId.ToString()).SendAsync("OnStartWork");
            
            if (!await TryLinkWithWaitingUser(supportId))
            {
                StoresForSupport.FreeSupports.Enqueue(supportId);
            }
            return;
        }
        
        if (StoresForSupport.ConnectionIdsByUserId.TryGetValue(supportId, out var connections))
        {
            await Clients.User(supportId.ToString()).SendAsync("OnStartWork");
            StoresForSupport.ConnectionIdsByUserId[supportId].Add(Context.ConnectionId);
            if (StoresForSupport.FreeSupports.Contains(supportId))
            {
                return;
            }
            
            var connection = connections.First();
            var groupName = StoresForSupport.GroupNameByConnectionId[connection];
            await AddConnectionToGroup(Context.ConnectionId, groupName);
            await PrepareChatAsync(groupName);
            return;
        }
        
        MapIdConnectionToUserId(supportId, Context.ConnectionId);
        
        await Clients.User(supportId.ToString()).SendAsync("OnStartWork");
        
        if (!await TryLinkWithWaitingUser(supportId))
        {
            StoresForSupport.FreeSupports.Enqueue(supportId);
        }
    }

    public void TryRegisterConnection()
    {
        var supportId = Context.User.GetUserId().Value;
        StoresForSupport.ConnectionIdsByUserId[supportId].Add(Context.ConnectionId);
    }
    
    public async Task ConnectUser()
    {
        var userId = Context.User?.GetUserId();

        if (userId is not null && 
            StoresForSupport.ConnectionIdsByUserId.TryGetValue(userId.Value, out var connections))
        {
            var connection = connections.First();
            var groupName = StoresForSupport.GroupNameByConnectionId[connection];
            await AddConnectionToGroup(Context.ConnectionId, groupName);
            StoresForSupport.ConnectionIdsByUserId[userId.Value].Add(Context.ConnectionId);
            return;
        }

        if (userId is not null)
        {
            MapIdConnectionToUserId(userId.Value, Context.ConnectionId);
        }

        var newGroupName = Context.User?.Identity?.Name ?? Context.ConnectionId;
        StoresForSupport.DisconnectedGroups.Remove(newGroupName);
        await AddConnectionToGroup(Context.ConnectionId, newGroupName);
        
        if (!await TryLinkWithFreeSupport(newGroupName))
        {
            StoresForSupport.WaitingGroupsWithUser.Enqueue(newGroupName);
        }
    }
    
    public async Task StopWorkForSupport()
    {
        var supportId = Context.User.GetUserId().Value;
        StoresForSupport.FreeSupports.Remove(supportId);
        await Clients.User(supportId.ToString()).SendAsync("OnStopWork");
    }

    public async Task SendUserInfoToSupport()
    {
        var userId = Context.User.GetUserId();
        var userInfo = userId is not null
            ? await _userService.GetUserInfoForChat(userId.Value)
            : null;

        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        
        await Clients.Group(groupName).SendAsync("PrepareUserInfoInChat", userInfo);
    }

    public void PutInQueueCurrentUser()
    {
        var groupName = StoresForSupport.GroupNameByConnectionId[Context.ConnectionId];
        StoresForSupport.WaitingGroupsWithUser.Enqueue(groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User;
        if (user is not null && user.HasRoleClaim(RoleNames.Support))
        {
            await DisconnectSupport();
        }
        else
        {
            await DisconnectUser();
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public override async Task OnConnectedAsync()
    {
        if (Context.User.HasRoleClaim(RoleNames.Support))
        {
            var supportId = Context.User.GetUserId()!.Value;
            if (StoresForSupport.ConnectionIdsByUserId.ContainsKey(supportId))
            {
                await Clients.Caller.SendAsync("ConnectToCurrentSession");   
            }
        }

        await base.OnConnectedAsync();
    }

    public async Task DisconnectFromChatWithUser()
    {
        var supportId = Context.User.GetUserId().Value;
        var supportConnections = StoresForSupport.ConnectionIdsByUserId[supportId];
        var groupName = StoresForSupport.GroupNameByConnectionId[supportConnections.First()];
        await Clients.Group(groupName).SendAsync("OnDisconnectSupport");
        
        foreach (var connection in supportConnections)
        {
            await TryRemoveConnectionFromGroupAsync(connection);
        }

        StoresForSupport.DisconnectedGroups.Add(groupName);
        var notification = _templatedNotificationService.GetValueTemplateByName(
            ChatTemplatedNotificationName.CompletedDialogueBySupport); 
        await Clients.Groups(groupName).SendAsync("ShowNotification", notification);

        if (!await TryLinkWithWaitingUser(supportId))
        {
            StoresForSupport.FreeSupports.Enqueue(supportId);
        }
    }
    
    private async Task DisconnectUser()
    {
        var userId = Context.User?.GetUserId();
        var currentConnection = Context.ConnectionId;
        if (userId is not null && 
            StoresForSupport.ConnectionIdsByUserId.TryGetValue(userId.Value, out  var userConnections))
        {
            userConnections.Remove(currentConnection);

            if (userConnections.Count != 0)
            {
                await TryRemoveConnectionFromGroupAsync(currentConnection);
                return;
            }

            StoresForSupport.ConnectionIdsByUserId.Remove(userId.Value);
        }
        
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(currentConnection, out var groupName))
        {
            await TryRemoveConnectionFromGroupAsync(currentConnection);
            
            var notification = _templatedNotificationService.GetValueTemplateByName(
                ChatTemplatedNotificationName.UserDisconnectFromChat);
            await Clients.Groups(groupName).SendAsync("ShowNotificationInSupportPage", notification);
            
            StoresForSupport.WaitingGroupsWithUser.Remove(groupName);
            StoresForSupport.DisconnectedGroups.Remove(groupName);
        }
    }
    
    private async Task DisconnectSupport()
    {
        var supportId = Context.User.GetUserId().Value;
        var connectionId = Context.ConnectionId;
        if (!StoresForSupport.ConnectionIdsByUserId.TryGetValue(supportId, out  var supportConnections))
        {
            return;
        }

        supportConnections.Remove(connectionId);
        if (supportConnections.Count != 0)
        {
            await TryRemoveConnectionFromGroupAsync(connectionId);
            return;
        }

        StoresForSupport.ConnectionIdsByUserId.Remove(supportId);
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(connectionId, out var groupName))
        {
            await TryRemoveConnectionFromGroupAsync(connectionId);

            var notification = _templatedNotificationService.GetValueTemplateByName(
                ChatTemplatedNotificationName.SupportSuddenlyDisconnected); 
            await Clients.Groups(groupName).SendAsync("ShowNotification", notification);

            if (!await TryLinkWithFreeSupport(groupName))
            {
                notification = _templatedNotificationService.GetValueTemplateByName(
                    ChatTemplatedNotificationName.AllOperatorsAreBusy);
                await Clients.Groups(groupName).SendAsync("ShowNotification", notification);
                await Clients.Groups(groupName).SendAsync("GetInWaitingQueue");
            }
            else
            {
                notification = _templatedNotificationService.GetValueTemplateByName(
                    ChatTemplatedNotificationName.TransferToAnotherOperator);
                await Clients.Groups(groupName).SendAsync("ShowNotification", notification);
            }
        }

        StoresForSupport.FreeSupports.Remove(supportId);
    }

    private async Task<bool> TryRemoveConnectionFromGroupAsync(string connectionId)
    {
        if (StoresForSupport.GroupNameByConnectionId.TryGetValue(connectionId, out var groupName))
        {
            StoresForSupport.GroupNameByConnectionId.Remove(connectionId);
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
            return true;
        }

        return false;
    }

    private async Task AddConnectionToGroup(string connectionId, string groupName)
    {
        StoresForSupport.GroupNameByConnectionId[connectionId] = groupName;
        await Groups.AddToGroupAsync(connectionId, groupName);
    }

    private async Task PrepareChatAsync(string groupName)
    {
        await Clients.Group(groupName).SendAsync("PrepareChat", groupName);
        await Clients.Group(groupName).SendAsync("GetUserInfo");
    }
    
    private void MapIdConnectionToUserId(Guid supportId, string connectionId)
    {
        StoresForSupport.ConnectionIdsByUserId.TryAdd(supportId, new HashSet<string>());
        StoresForSupport.ConnectionIdsByUserId[supportId].Add(connectionId);
    }

    private async Task<bool> TryLinkWithWaitingUser(Guid supportId)
    {
        if (!StoresForSupport.WaitingGroupsWithUser.TryDequeue(out var groupName))
        {
            return false;
        }

        var connectionIds = StoresForSupport.ConnectionIdsByUserId[supportId];
        foreach (var supportConnectionId in connectionIds)
        {
            await AddConnectionToGroup(supportConnectionId, groupName);
        }

        await PrepareChatAsync(groupName);

        return true;
    }

    private async Task<bool> TryLinkWithFreeSupport(string groupName)
    {
        if (!StoresForSupport.FreeSupports.TryDequeue(out var supportId))
        {
            return false;
        }

        var connectionIds = StoresForSupport.ConnectionIdsByUserId[supportId];
        foreach (var supportConnectionId in connectionIds)
        {
            await AddConnectionToGroup(supportConnectionId, groupName);
        }
        
        await PrepareChatAsync(groupName);

        return true;
    }
}