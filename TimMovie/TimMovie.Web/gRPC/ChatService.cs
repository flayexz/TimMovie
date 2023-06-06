using System.Collections.Concurrent;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TimMovie.SharedKernel.Extensions;

namespace TimMovie.Web.gRPC;

public class ChatService: Chat.ChatBase
{
    private static readonly ConcurrentQueue<ChatClient> FreeSupports = new();
    private static readonly ConcurrentQueue<ChatClient> FreeUsers = new();
    private static readonly ConcurrentDictionary<ChatClient, ChatClient> Chats = new();

    public override async Task ConnectSupportToChat(
        AttachedClient request,
        IServerStreamWriter<ChatMessage> responseStream,
        ServerCallContext context)
    {
        var support = new ChatClient(request.Name);
        if (FreeUsers.TryDequeue(out var freeUser))
        {
            Chats[support] = freeUser;
            freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.OldChat });
        }
        else
        {
            FreeSupports.Enqueue(support);
            support.Events.Enqueue(new ChatEvent { Body = "Ожидайте пользователя", Status = ChatEventStatus.OldChat });
        }

        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                while (support.Messages.TryDequeue(out var message))
                {
                    await responseStream.WriteAsync(message);
                }
                
                await Task.Delay(100);
            }
        }
        catch (OperationCanceledException)
        {
            
        }
        finally
        {
            FreeSupports.Remove(support);

            if (Chats.TryRemove(support, out var user))
            {
                if (FreeSupports.TryDequeue(out var freeSupport))
                {
                    Chats[freeSupport] = user;
                    user.Events.Enqueue(new ChatEvent { Body = "Вас переключили на другого шарика эйчарика", Status = ChatEventStatus.OldChat });
                    freeSupport.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.NewChat });
                }
                else
                {
                    FreeUsers.Enqueue(user);
                    user.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик отключился, ожидайте нового", Status = ChatEventStatus.OldChat });
                }
            }
        }
    }

    public override async Task ConnectUserToChat(
        AttachedClient request,
        IServerStreamWriter<ChatMessage> responseStream,
        ServerCallContext context)
    {
        var user = new ChatClient(request.Name);
        if (FreeSupports.TryDequeue(out var freeSupport))
        {
            Chats[freeSupport] = user;
            freeSupport.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.OldChat });
        }
        else
        {
            FreeUsers.Enqueue(user);
            user.Events.Enqueue(new ChatEvent { Body = "Ожидайте шарика эйчарика", Status = ChatEventStatus.OldChat });
        }

        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                while (user.Messages.TryDequeue(out var message))
                {
                    await responseStream.WriteAsync(message);
                }
                
                await Task.Delay(100);
            }
        }
        catch (OperationCanceledException)
        {
            
        }
        finally
        {
            FreeUsers.Remove(user);

            KeyValuePair<ChatClient,ChatClient>? chat = Chats.FirstOrDefault(chat => chat.Value.Equals(user));
            if (chat is not null)
            {
                Chats.TryRemove(chat.Value);
                // находим нового пользователя
                var support = chat.Value.Key;
                if (FreeUsers.TryDequeue(out var freeUser))
                {
                    Chats[support] = freeUser;
                    support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, вас подключили к новому пользователю", Status = ChatEventStatus.NewChat });
                    freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.OldChat });
                }
                else
                {
                    FreeSupports.Enqueue(support);
                    support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, ожидайте нового пользователя", Status = ChatEventStatus.NewChat });
                }
            }
        }
    }

    public override Task<Empty> SendMessage(ChatMessage request, ServerCallContext context)
    {
        var client = new ChatClient(request.Name);
        var message = new ChatMessage { Body = request.Body, Name = request.Name };

        KeyValuePair<ChatClient,ChatClient>? chat = Chats.FirstOrDefault(chat => chat.Key.Equals(client) || chat.Value.Equals(client));
        if (chat is not null)
        {
            chat.Value.Key.Messages.Enqueue(message);
            chat.Value.Value.Messages.Enqueue(message);
            return Task.FromResult(new Empty());
        }

        if (client.IsAdmin)
        {
            var support = FreeSupports.First(s => s.Equals(client));
            support.Events.Enqueue(new ChatEvent { Body = "Ожидайте клиента", Status = ChatEventStatus.NewChat });
        }
        if (!client.IsAdmin)
        {
            var user = FreeUsers.First(s => s.Equals(client));
            user.Events.Enqueue(new ChatEvent { Body = "Ожидайте шарика эйчарика", Status = ChatEventStatus.OldChat });
        }
        
        return Task.FromResult(new Empty());
    }

    public override async Task ConnectToEvents(
        AttachedClient request,
        IServerStreamWriter<ChatEvent> responseStream,
        ServerCallContext context)
    {
        var client = new ChatClient(request.Name);
        KeyValuePair<ChatClient,ChatClient>? oldClient = Chats
            .FirstOrDefault(c => c.Key.Equals(client) || c.Value.Equals(client));
        if (client.IsAdmin)
        {
            client = oldClient?.Key ?? FreeSupports.First(s => s.Equals(client));
        }
        else
        {
            client = oldClient?.Value ?? FreeUsers.First(s => s.Equals(client));
        }

        try
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                while (client.Events.TryDequeue(out var e))
                {
                    await responseStream.WriteAsync(e);
                }
                
                await Task.Delay(100);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public override Task<Empty> DisconnectSupportFromChat(AttachedClient request, ServerCallContext context)
    {
        var support = new ChatClient(request.Name);
        support = Chats.First(c => c.Key.Equals(support)).Key;

        if (Chats.TryRemove(support, out var user))
        {
            user.Events.Enqueue(new ChatEvent
            {
                Body = "Шарик эйчарик отключился, если есть вопросы, перезайдите в чат", 
                Status = ChatEventStatus.OldChat
            });

            if (FreeUsers.TryDequeue(out var freeUser))
            {
                Chats[support] = freeUser;
                freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.OldChat });
                support.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.NewChat });
            }
            else
            {
                FreeSupports.Enqueue(support);
                support.Events.Enqueue(new ChatEvent { Body = "Ожидайте пользователя", Status = ChatEventStatus.NewChat });
            }
        }

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> DisconnectUserFromChat(AttachedClient request, ServerCallContext context)
    {
        var user = new ChatClient(request.Name);
        
        KeyValuePair<ChatClient,ChatClient>? chat = Chats.FirstOrDefault(chat => chat.Value.Equals(user));
        user = chat?.Value ?? FreeUsers.First(u => u.Equals(user));
        
        FreeUsers.Remove(user);

        if (chat is not null)
        {
            Chats.TryRemove(chat.Value);
            // находим нового пользователя
            var support = chat.Value.Key;
            if (FreeUsers.TryDequeue(out var freeUser))
            {
                Chats[support] = freeUser;
                support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, вас подключили к новому пользователю", Status = ChatEventStatus.NewChat });
            }
            else
            {
                FreeSupports.Enqueue(support);
                support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, ожидайте нового пользователя", Status = ChatEventStatus.NewChat });
            }
        }
        
        return Task.FromResult(new Empty());
    }
}