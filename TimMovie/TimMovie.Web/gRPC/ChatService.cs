using System.Collections.Concurrent;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using TimMovie.SharedKernel.Extensions;
using TimMovie.Web.Extensions;

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
            freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.Simple });
            support.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.UserConnectToChat });
        }
        else
        {
            FreeSupports.Enqueue(support);
            support.Events.Enqueue(new ChatEvent { Body = "Ожидайте пользователя", Status = ChatEventStatus.Simple });
        }

        try
        {
            await responseStream.WriteAsync(new ChatMessage {Body = "Инициализация", Name = "init"});
            await Task.Delay(1000);
            while (!context.CancellationToken.IsCancellationRequested)
            {
                while (support.Messages.TryDequeue(out var message))
                {
                    await responseStream.WriteAsync(message);
                }

                await Task.Delay(1000);
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
                    user.Events.Enqueue(new ChatEvent { Body = "Вас переключили на другого шарика эйчарика", Status = ChatEventStatus.Simple });
                    freeSupport.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.UserConnectToChat });
                }
                else
                {
                    FreeUsers.Enqueue(user);
                    user.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик отключился, ожидайте нового", Status = ChatEventStatus.Simple });
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
            freeSupport.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.UserConnectToChat });
            user.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.Simple });
        }
        else
        {
            FreeUsers.Enqueue(user);
            user.Events.Enqueue(new ChatEvent { Body = "Ожидайте шарика эйчарика", Status = ChatEventStatus.Simple });
        }

        try
        {
            await responseStream.WriteAsync(new ChatMessage {Body = "Инициализация", Name = "init"});
            await Task.Delay(1000);
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

            var chat = Chats.FirstOrDefault(chat => chat.Value.Equals(user));
            if (chat.IsNotDefault())
            {
                Chats.TryRemove(chat);
                var support = chat.Key;
                if (FreeUsers.TryDequeue(out var freeUser))
                {
                    Chats[support] = freeUser;
                    support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, вас подключили к новому пользователю", Status = ChatEventStatus.UserConnectToChat });
                    freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.Simple });
                }
                else
                {
                    FreeSupports.Enqueue(support);
                    support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, ожидайте нового пользователя", Status = ChatEventStatus.UserDisconnectFromChat });
                }
            }
        }
    }

    public override Task<Empty> SendMessage(ChatMessage request, ServerCallContext context)
    {
        var client = new ChatClient(request.Name);
        var message = new ChatMessage { Body = request.Body, Name = request.Name };

        var chat = Chats
            .FirstOrDefault(chat => chat.Key.Equals(client) || chat.Value.Equals(client));
        if (chat.IsNotDefault())
        {
            chat.Key.Messages.Enqueue(message);
            chat.Value.Messages.Enqueue(message);
            return Task.FromResult(new Empty());
        }

        if (client.IsAdmin)
        {
            var support = FreeSupports.First(s => s.Equals(client));
            support.Events.Enqueue(new ChatEvent { Body = "Ожидайте клиента", Status = ChatEventStatus.Simple });
        }
        if (!client.IsAdmin)
        {
            var user = FreeUsers.First(s => s.Equals(client));
            user.Events.Enqueue(new ChatEvent { Body = "Ожидайте шарика эйчарика", Status = ChatEventStatus.Simple });
        }
        
        return Task.FromResult(new Empty());
    }

    public override async Task ConnectToEvents(
        AttachedClient request,
        IServerStreamWriter<ChatEvent> responseStream,
        ServerCallContext context)
    {
        var client = new ChatClient(request.Name);
        var (support, user) = Chats
            .FirstOrDefault(c => c.Key.Equals(client) || c.Value.Equals(client));
        if (client.IsAdmin)
        {
            client = support ?? FreeSupports.First(s => s.Equals(client));
        }
        else
        {
            client = user ?? FreeUsers.First(s => s.Equals(client));
        }

        try
        {
            await responseStream.WriteAsync(new ChatEvent {Body = "Инициализация", Status = ChatEventStatus.Simple});
            await Task.Delay(1000);
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
                Status = ChatEventStatus.Simple
            });

            if (FreeUsers.TryDequeue(out var freeUser))
            {
                Chats[support] = freeUser;
                freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.Simple });
                support.Events.Enqueue(new ChatEvent { Body = "Пользователь подключен", Status = ChatEventStatus.UserConnectToChat });
            }
            else
            {
                FreeSupports.Enqueue(support);
                support.Events.Enqueue(new ChatEvent { Body = "Ожидайте пользователя", Status = ChatEventStatus.Simple });
            }
        }

        return Task.FromResult(new Empty());
    }

    public override Task<Empty> DisconnectUserFromChat(AttachedClient request, ServerCallContext context)
    {
        var user = new ChatClient(request.Name);
        
        var chat = Chats.FirstOrDefault(chat => chat.Value.Equals(user));
        user = chat.Value ?? FreeUsers.First(u => u.Equals(user));
        
        FreeUsers.Remove(user);

        if (chat.IsNotDefault())
        {
            Chats.TryRemove(chat);
            var support = chat.Key;
            if (FreeUsers.TryDequeue(out var freeUser))
            {
                Chats[support] = freeUser;
                support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, вас подключили к новому пользователю", Status = ChatEventStatus.UserConnectToChat });
                freeUser.Events.Enqueue(new ChatEvent { Body = "Шарик эйчарик подключился", Status = ChatEventStatus.Simple });
            }
            else
            {
                FreeSupports.Enqueue(support);
                support.Events.Enqueue(new ChatEvent { Body = "Пользователь отключился, ожидайте нового пользователя", Status = ChatEventStatus.UserDisconnectFromChat });
            }
        }
        
        return Task.FromResult(new Empty());
    }
}