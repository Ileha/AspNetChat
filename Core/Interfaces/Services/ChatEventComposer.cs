using AspNetChat.Core.Interfaces.ChatEvents;
using Newtonsoft.Json;

namespace AspNetChat.Core.Interfaces.Services
{
	public class ChatEventComposer 
	{
		public IEnumerable<BaseUserEvent> GetEvents(IReadOnlyList<IEvent> events) 
		{
			var visiter = new MessageCollector();

			foreach (var @event in events)
			{
				visiter.Clear();

				@event.Accept(visiter);

				yield return visiter.UserEvent!;
			}
		}

		public enum UserEventType
		{
			Joined = 0,
			Message = 1,
			Disconnected = 2,
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

		private class MessageCollector : IEventVisitor
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
	}
}
