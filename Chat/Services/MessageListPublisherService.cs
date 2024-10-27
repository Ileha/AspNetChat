using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using Chat.Extensions.Comparers;
using Chat.Interfaces;
using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services;
using Common.Entities;
using Common.Extensions;
using Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chat.Services
{
    public class MessageListPublisherService : IMessageListPublisherService, IMessageConsumerService, IDisposable
	{
		private readonly IChatContainer _chatContainer;
		private readonly ChatUserHelper _chatUserHelper;
		private readonly ChatEventComposer _chatEventComposer;
		private readonly ILogger<MessageListPublisherService> _logger;

		/// <summary>
		/// key is chat
		/// </summary>
		private readonly ConcurrentDictionary<IIdentifiable, ChatData> _allChatsData
			= new ConcurrentDictionary<IIdentifiable, ChatData>(new IdentifiableEqualityComparer());
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly CancellationToken _cancellationToken;

		public MessageListPublisherService(
			IChatContainer chatContainer, 
			ChatUserHelper chatUserHelper,
			ChatEventComposer chatEventComposer,
			ILogger<MessageListPublisherService> logger)
		{
			_chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
			_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
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

		public async Task ConnectWebSocket(string userID, string chatID, HttpContext context)
		{
			var message = string.Empty;
			var statusCode = HttpStatusCode.InternalServerError;

			if (!_chatUserHelper.GetUserAndChatID(userID, chatID, out statusCode, out message, out var userGuid, out var chatGuid)) 
			{
				context.Response.StatusCode = (int) statusCode;
				await context.Response.WriteAsync(message);

				return;
			}

			if (!_chatUserHelper.GetUserAndChat(userGuid, chatGuid, out statusCode, out message, out var chat)) 
			{
				context.Response.StatusCode = (int)statusCode;
				await context.Response.WriteAsync(message);

				return;
			}

			var wsContext = new WebSocketAcceptContext()
			{
				KeepAliveInterval = TimeSpan.FromMinutes(1),
			};
			var webSocket = await context.WebSockets.AcceptWebSocketAsync(wsContext);
			
			var chatData = _allChatsData.AddOrUpdate(
				chat,
				(chatId) =>
				{
					return new ChatData(
						new ConcurrentDictionary<IIdentifiable, UserConnection>(new IdentifiableEqualityComparer())
					);
				},
				(chatId, item) => item);

			var userConnection = new UserConnection(webSocket, chat, (Identifiable)userGuid, _cancellationToken);

			var actualUserConnection = chatData.Connections.AddOrUpdate((Identifiable)userGuid, userConnection, 
				(_, oldUserConnection) => 
				{
					oldUserConnection.Dispose();

					return userConnection;
				}
			);

			await HandleUserWebSocket(actualUserConnection, userConnection.CancellationToken);
		}

		public void Dispose()
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		private async Task HandleUserWebSocket(UserConnection connection, CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					var message = await connection.WebSocket.WaitMessageAsync(token);
				}
				catch (OperationCanceledException)
				{
					if (!_allChatsData.TryGetValue(connection.Chat, out var chatData)) 
					{
						_logger.LogWarning($"{nameof(HandleUserWebSocket)}: unable to find chat for user {connection.User.Id}");
						return;
					}

					if (!chatData.Connections.TryRemove(connection.User, out _)) 
					{
						_logger.LogWarning($"{nameof(HandleUserWebSocket)}: unable to remove user {connection.User.Id} " +
							$"from chat connection {connection.Chat}");
					}

					return;
				}
			}
		}

		private class UserConnection : IDisposable
		{
			public WebSocket WebSocket { get; }
			public IIdentifiable Chat { get; }
			public IIdentifiable User { get; }
			public CancellationToken CancellationToken { get; }

			private readonly CancellationTokenSource _cancellationTokenSource;

			public UserConnection(
				WebSocket webSocket, 
				IIdentifiable chat, 
				IIdentifiable user,
				CancellationToken token)
			{
				WebSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
				Chat = chat ?? throw new ArgumentNullException(nameof(chat));
				User = user ?? throw new ArgumentNullException(nameof(user));


				_cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
				CancellationToken = _cancellationTokenSource.Token;
			}

			public void Dispose()
			{
				WebSocket.Dispose();
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource.Dispose();
			}
		}

		private record ChatData(ConcurrentDictionary<IIdentifiable, UserConnection> Connections);
	}
}
