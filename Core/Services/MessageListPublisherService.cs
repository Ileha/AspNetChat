using AspNetChat.Core.Entities;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services;
using AspNetChat.Extensions;
using AspNetChat.Extensions.Comparers;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;

namespace AspNetChat.Core.Services
{
	public class MessageListPublisherService : IMessageListPublisherService, IMessageConsumerService, IDisposable
	{
		private readonly IChatContainer _chatContainer;
		private readonly ChatUserHelper _chatUserHelper;

		/// <summary>
		/// key is chat
		/// </summary>
		private readonly ConcurrentDictionary<IIdentifiable, ChatData> _allChatsData
			= new ConcurrentDictionary<IIdentifiable, ChatData>(new IdentifiableEqualityComparer());
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly CancellationToken _cancellationToken;

		public MessageListPublisherService(
			IChatContainer chatContainer, 
			ChatUserHelper chatUserHelper)
		{
			_chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
			_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
			_cancellationToken = _cancellationTokenSource.Token;
		}

		public async void ConsumeMessage(IChat chat, IReadOnlyList<IEvent> events)
		{
			if (!_allChatsData.TryGetValue(chat, out var chatData))
				return;

			var eventsList = GetUserEventDataObject(events).ToArray();

			var data = JsonConvert.SerializeObject(eventsList);

			await Task.WhenAll(
				chatData.Connections.Values.Select(item => item.WebSocket.SendMessageAsync(data, _cancellationToken))
			);
		}

		public async Task ConectWebSocket(string userID, string chatID, HttpContext context)
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

			var webSocket = await context.WebSockets.AcceptWebSocketAsync();

			var chatData = _allChatsData.AddOrUpdate(
				chat,
				(chatId) =>
				{
					return new ChatData(
						new ConcurrentDictionary<IIdentifiable, UserConnection>()
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

			HandleUserWebSocket(actualUserConnection, userConnection.CancellationToken);
		}

		public void Dispose()
		{
			_cancellationTokenSource.Cancel();
			_cancellationTokenSource.Dispose();
		}

		private IEnumerable<BaseUserEvent> GetUserEventDataObject(IReadOnlyList<IEvent> events) 
		{
			var visiter = new MessageCollector();

			foreach (var @event in events)
			{
				visiter.Clear();

				@event.Accept(visiter);

				yield return visiter.UserEvent!;
			}
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
						return;

					chatData.Connections.TryRemove(connection.User, out _);

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

		public enum UserEventType
		{
			Joined = 0,
			Message = 1,
			Disconnected = 2,
		}

		public class MessageCollector : IEventVisitor
		{
			public BaseUserEvent? UserEvent { get; private set; }

			public void Visit(IUserConnected userConnected)
			{
				UserEvent = new UserJoined(userConnected.UserName, userConnected.DateTime, userConnected.User.Id);
			}

			public void Visit(IUserSendMessage userSendMessage)
			{
				UserEvent = new UserSendMessage(userSendMessage.Message, userSendMessage.DateTime, userSendMessage.User.Id);
			}

			public void Visit(IUserDisconnected userDisconnected)
			{
				UserEvent = new UserDisconnected(userDisconnected.DateTime, userDisconnected.User.Id);
			}

			public void Clear() 
			{
				UserEvent = null;
			}
		}

		public record BaseUserEvent(
			[JsonProperty("time")]
			DateTime Time,
			[JsonProperty("eventType")]
			UserEventType EventType,
			[JsonProperty("userID")]
			Guid UserId);

		public record UserJoined(
			[JsonProperty("userName")]
			string Name,
			DateTime Time,
			Guid UserId) 
			: BaseUserEvent(Time, UserEventType.Joined, UserId);

		public record UserSendMessage(
			[JsonProperty("message")]
			string Message,
			DateTime Time,
			Guid UserId) 
			: BaseUserEvent(Time, UserEventType.Message, UserId);

		public record UserDisconnected(
			DateTime Time,
			Guid UserId) 
			: BaseUserEvent(Time, UserEventType.Disconnected, UserId);
	}
}
