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
using System.Text;

namespace AspNetChat.Core.Services
{
	public class MessageListPublisherService : IMessageListPublisherService, IMessageConsumerService, IDisposable
	{
		private readonly IChatContainer _chatContainer;
		/// <summary>
		/// key is chat
		/// </summary>
		private readonly ConcurrentDictionary<IIdentifiable, ChatData> _allChatsData
			= new ConcurrentDictionary<IIdentifiable, ChatData>(new IdentifiableEqualityComparer());
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly CancellationToken _cancellationToken;

		public MessageListPublisherService(IChatContainer chatContainer)
		{
			_chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
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

		public async Task InvokeAsync(string userID, string chatID, HttpContext context)
		{
			if (!Guid.TryParse(chatID, out var chatGuid))
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				await context.Response.WriteAsync("incorrect chat id");
				return;
			}

			if (!Guid.TryParse(userID, out var userGuid))
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				await context.Response.WriteAsync("incorrect user id");
				return;
			}

			if (!_chatContainer.HasChat(chatGuid))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				await context.Response.WriteAsync("chat not found");
				return;
			}

			var chat = _chatContainer.GetChatById(chatGuid);

			if (!chat.HasPartisipant((Identifiable)userGuid))
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				await context.Response.WriteAsync("user not found");
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

			var userConnection = new UserConnection(webSocket, chat, (Identifiable)userGuid);

			var actualUserConnection = chatData.Connections.AddOrUpdate((Identifiable)userGuid, userConnection, 
				(_, oldUserConnection) => 
				{
					oldUserConnection.Dispose();

					return userConnection;
				}
			);

			HandleUserWebSocket(actualUserConnection, _cancellationToken);
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

		private record UserConnection(
			WebSocket WebSocket,
			IIdentifiable Chat,
			IIdentifiable User)
			: IDisposable
		{
			public void Dispose()
			{
				WebSocket.Dispose();
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
