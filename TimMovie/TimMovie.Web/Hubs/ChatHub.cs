using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TimMovie.Core;
using TimMovie.Core.Classes;
using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Interfaces;
using TimMovie.Web.Extensions;

namespace TimMovie.Web.Hubs;

public class ChatHub : Hub
{
    private readonly IRepository<Message> _messageRepository;
    private readonly UserManager<User> _userManager;

    public ChatHub(IRepository<Message> messageRepository, UserManager<User> userManager)
    {
        _messageRepository = messageRepository;
        _userManager = userManager;
    }

    public async Task SendMessageToUser(string content)
    {
        var support = await _userManager.GetUserAsync(Context.User);
        var userConnectInfo = StoresForSupport.CurrentCommunicationForSupport[(support.Id, Context.ConnectionId)];
        var user = userConnectInfo.userId is not null
            ? await _userManager.FindByIdAsync(userConnectInfo.userId?.ToString())
            : null;
        var newMessage = new Message
        {
            ToUser = true,
            Content = content,
            Date = DateTime.UtcNow,
            User = user,
            Support = support
        };
        await _messageRepository.AddAsync(newMessage);
        await _messageRepository.SaveChangesAsync();

        var messageWithContent = new MessageWithContent()
        {
            Content = content,
            ToUser = true
        };

        //Возможно нужно проверять на то, что пользователь отключился.
        await Clients.Client(userConnectInfo.userConnectionId).SendAsync("ReceiveNewMessage", messageWithContent);
        await Clients.Caller.SendAsync("ReceiveNewMessage", messageWithContent);
    }

    public async Task SendMessageToSupport(string content)
    {
        var user = Context.User.Identity.IsAuthenticated 
            ? await _userManager.GetUserAsync(Context.User) 
            : null;
        var userConnectionInfo = (user?.Id, Context.ConnectionId);
        User? support = null;
        if (StoresForSupport.CurrentCommunicationForSupport.Reverse.TryGetValue(
                userConnectionInfo,
                out var supportConnectInfo))
        {
            support = await _userManager.FindByIdAsync(supportConnectInfo.supportId.ToString());
        }
        else
        {
            StoresForSupport.WaitingUsers.Add(userConnectionInfo);
        }
        
        var newMessage = new Message
        {
            ToUser = false,
            Content = content,
            Date = DateTime.UtcNow,
            User = user,
            Support = support
        };
        await _messageRepository.AddAsync(newMessage);
        await _messageRepository.SaveChangesAsync();

        var messageWithContent = new MessageWithContent()
        {
            Content = content,
            ToUser = false
        };

        if (support is not null)
        {
            await Clients
                .Client(supportConnectInfo.supportConectionId)
                .SendAsync("ReceiveNewMessage", messageWithContent);    
        }
        
        await Clients.Caller.SendAsync("ReceiveNewMessage", messageWithContent);
    }

    public async Task ConnectSupport()
    {
        var supportId = Context.User.GetUserId().Value;
        if (!await TryLinkToWaitingUser(supportId))
        {
            StoresForSupport.FreeSupports.Add((supportId, Context.ConnectionId));
        }
    }

    public async Task ConnectUser()
    {
        var userId = Context.User?.GetUserId();
        if (!await TryLinkToFreeSupport(userId))
        {
            StoresForSupport.WaitingUsers.Add((userId, Context.ConnectionId));
        }
    }

    public async Task SendMessagesFromUnauthorisedUser(MessageWithContent[] messages, string supportConnectionId)
    {
        await Clients.Client(supportConnectionId).SendAsync("DownloadMessagesFromUnauthorisedUser", messages);
    }

    private async Task<bool> TryLinkToWaitingUser(Guid supportId)
    {
        if (!StoresForSupport.WaitingUsers.Any())
        {
            return false;
        }

        var user = StoresForSupport.WaitingUsers.First();
        StoresForSupport.WaitingUsers.Remove(user);

        StoresForSupport.CurrentCommunicationForSupport.Add(
            (supportId, Context.ConnectionId), (user.userId, user.userConnectionId));

        if (user.userId is not null)
        {
            await Clients.Caller.SendAsync("DownloadMessagesFromDb", user.userId);
        }
        else
        {
            await Clients.Client(user.userConnectionId).SendAsync("GetMessages", Context.ConnectionId);
        }

        return true;
    }

    private async Task<bool> TryLinkToFreeSupport(Guid? userId)
    {
        if (!StoresForSupport.FreeSupports.Any())
        {
            return false;
        }

        var support = StoresForSupport.FreeSupports.First();
        StoresForSupport.FreeSupports.Remove(support);

        StoresForSupport.CurrentCommunicationForSupport.Add(
            (support.supportId, support.supportConectionId), (userId, Context.ConnectionId));

        if (userId is not null)
        {
            await Clients.Client(support.supportConectionId).SendAsync("DownloadMessagesFromDb", userId.Value);
        }
        else
        {
            await Clients.Caller.SendAsync("GetMessages", support.supportConectionId);
        }

        return true;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User;
        if (user is not null && user.FindAll(ClaimTypes.Role).Any(claim => claim.Value == "support"))
        {
            await DisconnectSupport();
        }
        else
        {
            await DisconnectUser();
        }
        
        await base.OnDisconnectedAsync(exception);
    }

    public async Task DisconnectFromChatWithUser()
    {
        var supportCommunicationInfo = (supportId: Context.User.GetUserId().Value, Context.ConnectionId);
        var userConnectionInfo = StoresForSupport.CurrentCommunicationForSupport[supportCommunicationInfo];
        StoresForSupport.CurrentCommunicationForSupport.Remove(supportCommunicationInfo);
        StoresForSupport.WaitingUsers.Remove(userConnectionInfo);
        var messageWithContent = new MessageWithContent
        {
            Content = "Оператор вышел из чата." +
                      "Если у вас остались еще вопросы, то напишите сообщение.",
            ToUser = true
        };
        await Clients.Client(userConnectionInfo.userConnectionId).SendAsync("ReceiveNewMessage", messageWithContent);
        
        if (!await TryLinkToWaitingUser(supportCommunicationInfo.supportId))
        {
            StoresForSupport.FreeSupports.Add(supportCommunicationInfo);
        }
    }

    private async Task DisconnectUser()
    {
        var userConnectionInfo = (userId: Context.User?.GetUserId(), Context.ConnectionId);
        StoresForSupport.WaitingUsers.Remove(userConnectionInfo);

        if (StoresForSupport.CurrentCommunicationForSupport.Reverse.TryGetValue(
                userConnectionInfo,
                out var supportConnectionInfo))
        {
            await Clients.Client(supportConnectionInfo.supportConectionId).SendAsync("OnUserDisconnect");
        }
    }
    
    private async Task DisconnectSupport()
    {
        var supportId = Context.User.GetUserId().Value;
        var supportConnectionInfo = (supportId, Context.ConnectionId);
        if (StoresForSupport.CurrentCommunicationForSupport.ContainsKey(supportConnectionInfo))
        {
            var userConnectionInfo =
                StoresForSupport.CurrentCommunicationForSupport[supportConnectionInfo];
            
            var messageWithContent = new MessageWithContent
            {
                Content = "Человек из Ильдар-техподдержки отключился." +
                          " Вам подпбирается человек, который бы смог ответить на ваши вопросы.",
                ToUser = true
            };

            await Clients.Client(userConnectionInfo.userConnectionId)
                .SendAsync("ReceiveNewMessage", messageWithContent);
            StoresForSupport.CurrentCommunicationForSupport.Remove(supportConnectionInfo);
            
            if (!await TryLinkToFreeSupport(userConnectionInfo.userId))
            {
                messageWithContent.Content = "К сожалению все операторы пока что заняты. Ожидайте.";
                await Clients.Client(userConnectionInfo.userConnectionId)
                    .SendAsync("ReceiveNewMessage", messageWithContent);
                StoresForSupport.WaitingUsers.Add(userConnectionInfo);
            }
            else
            {
                messageWithContent.Content = "Мы перевели вас на другого оператора.";
                await Clients.Client(userConnectionInfo.userConnectionId)
                    .SendAsync("ReceiveNewMessage", messageWithContent);
            }
        }

        StoresForSupport.FreeSupports.Remove(supportConnectionInfo);
    }
}