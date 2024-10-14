using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Extensions.Converters;
using Newtonsoft.Json;

namespace AspNetChat.Core.Services
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

        public abstract class BaseUserEvent
        {
			[JsonProperty("time")]
            [JsonConverter(typeof(DateTimeConverter))]
            public DateTime Time { get; set; }
	
			[JsonProperty("eventType")]
			public UserEventType EventType { get; set; }

			[JsonProperty("userID")]
			public Guid UserId { get; set; }

		}

        public class UserJoined : BaseUserEvent
        {
			[JsonProperty("userName")]
            public string Name { get; set; }
		}

        public class UserSendMessage : BaseUserEvent
		{
			[JsonProperty("message")]
			public string Message { get; set; }
		}

        public class UserDisconnected : BaseUserEvent 
        {
        
        }


		private class MessageCollector : IEventVisitor
        {
            public BaseUserEvent? UserEvent { get; private set; }

            public void Visit(IUserConnected userConnected)
            {
                UserEvent = new UserJoined()
                {
                    Time = userConnected.DateTime,
                    EventType = UserEventType.Joined,
                    UserId = userConnected.User.Id,
                    Name = userConnected.UserName,
                };
            }

            public void Visit(IUserSendMessage userSendMessage)
            {
				UserEvent = new UserSendMessage()
				{
					Time = userSendMessage.DateTime,
					EventType = UserEventType.Message,
					UserId = userSendMessage.User.Id,
					Message = userSendMessage.Message,
				};
			}

            public void Visit(IUserDisconnected userDisconnected)
            {
				UserEvent = new UserDisconnected()
				{
					Time = userDisconnected.DateTime,
					EventType = UserEventType.Disconnected,
					UserId = userDisconnected.User.Id,
				};
			}

            public void Clear()
            {
                UserEvent = null;
            }
        }
    }
}
