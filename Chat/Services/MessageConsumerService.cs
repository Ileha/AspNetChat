using System.Collections.Concurrent;
using System.Net.WebSockets;
using Chat.Entities;
using Chat.Extensions.Comparers;
using Chat.Interfaces;
using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services;
using Common.Entities;
using Common.Extensions;
using Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nito.Disposables;

namespace Chat.Services;

public class MessageConsumerService : IMessageConsumerService, IDisposable, IUserConnectionService
{
    /// <summary>
    /// key is chat
    /// </summary>
    private readonly ConcurrentDictionary<IIdentifiable, ChatData> _allChatsData = new(new IdentifiableEqualityComparer());
    private readonly ChatEventComposer _chatEventComposer;
    private readonly ILogger<MessageConsumerService> _logger;
	
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly CancellationToken _cancellationToken;

    public MessageConsumerService(
        ChatEventComposer chatEventComposer,
        ILogger<MessageConsumerService> logger)
    {
        _chatEventComposer = chatEventComposer ?? throw new ArgumentNullException(nameof(chatEventComposer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cancellationToken = _cancellationTokenSource.Token;
    }

    public async void ConsumeMessage(IChat chat, IReadOnlyList<IEvent> events)
    {
        if (!_allChatsData.TryGetValue(chat, out var chatData))
            return;

        try
        {
            var eventsList = _chatEventComposer.GetEvents(events).ToArray();

            var data = JsonConvert.SerializeObject(eventsList);

            await Task.WhenAll(
                chatData.Connections.Values.Select(item => item.WebSocket.SendMessageAsync(data, _cancellationToken))
            );
        }
        catch (Exception error)
        {
            _logger.LogError($"{nameof(ConsumeMessage)}: {error.ToString()}");
        }
    }
	
    public UserConnection AddUserConnection(IIdentifiable chat, IIdentifiable user, WebSocket webSocket)
    {
        var chatData = _allChatsData.AddOrUpdate(
            chat,
            _ => new ChatData(
                new ConcurrentDictionary<IIdentifiable, UserConnection>(new IdentifiableEqualityComparer())
            ),
            (_, item) => item);
        
        var connectionGuid = Guid.NewGuid();

        var userConnection = new UserConnection(
            connectionGuid, 
            webSocket, 
            chat, 
            user, 
            _cancellationToken, 
            new Disposable(() => RemoveUserConnection((Identifiable) connectionGuid, chat, user)));
        
        var actualUserConnection = chatData.Connections.AddOrUpdate(
            userConnection, 
            userConnection, 
            (_, oldUserConnection) => 
            {
                oldUserConnection.Dispose();
        
                return userConnection;
            }
        );

        return actualUserConnection;
    }
	
    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }

    private void RemoveUserConnection(IIdentifiable userConnection, IIdentifiable chat, IIdentifiable user)
    {
        if (!_allChatsData.TryGetValue(chat, out var chatData)) 
        {
            _logger.LogWarning($"{nameof(RemoveUserConnection)}: unable to find chat for user {user.Id}");
            return;
        }

        if (!chatData.Connections.TryRemove(userConnection, out _)) 
        {
            _logger.LogWarning($"{nameof(RemoveUserConnection)}: unable to remove user's connection {user.Id} " +
                               $"from chat connection {chat}");
        }
    }

    private record ChatData(ConcurrentDictionary<IIdentifiable, UserConnection> Connections);
}