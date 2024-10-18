using AspNetChat.Core.Entities.ChatModel.Events;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Extensions.Comparers;
using AspNetChat.Extensions.Converters;
using Newtonsoft.Json;
using static AspNetChat.Core.Services.ChatEventComposer;

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
            [JsonConverter(typeof(GuidConverter))]
			public Guid UserId { get; set; }

            [JsonProperty("eventID")]
			[JsonConverter(typeof(GuidConverter))]
			public Guid EventId { get; set; }
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
            private readonly Dictionary<string, int> _nameAmount = new Dictionary<string, int>();
            private readonly Dictionary<IIdentifiable, UserData> _id2Name = 
                new Dictionary<IIdentifiable, UserData>(new IdentifiableEqualityComparer());

            public BaseUserEvent? UserEvent { get; private set; }

            public void Visit(IUserConnected userConnected)
            {
                AddUser(userConnected.User, userConnected.UserName);

                if (!TryGetMappedName(userConnected.User, out var mappedName))
                    throw new InvalidOperationException($"unable to find mapped name for user with if {userConnected.User.Id}");


				UserEvent = new UserJoined()
                {
                    Time = userConnected.DateTime,
                    EventType = UserEventType.Joined,
                    UserId = userConnected.User.Id,
                    Name = mappedName,
                    EventId = userConnected.Id,
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
					EventId = userSendMessage.Id,
				};
			}

            public void Visit(IUserDisconnected userDisconnected)
            {
				UserEvent = new UserDisconnected()
				{
					Time = userDisconnected.DateTime,
					EventType = UserEventType.Disconnected,
					UserId = userDisconnected.User.Id,
					EventId = userDisconnected.Id,
				};
			}

            public void Clear()
            {
                UserEvent = null;
            }

            private bool TryGetMappedName(IIdentifiable user, out string name) 
            {
                name = string.Empty;

                if (!_id2Name.TryGetValue(user, out var userData))
                    return false;

                name = userData.MappedName;

                return true;
            }

            private void AddUser(IIdentifiable user, string name) 
            {
                if (_id2Name.ContainsKey(user))
                    throw new InvalidOperationException($"name {name} already assigned for user with id {user.Id}");

                var userAmount = GetNameAmount(name);

                _id2Name[user] = new UserData(name, userAmount > 0 ? $"{name} ({userAmount})" : name);
                AddNameAmount(name, 1);
            }

            private void AddNameAmount(string name, int amount) 
            {
                if (!_nameAmount.TryGetValue(name, out var currentAmount)) 
                {
                    currentAmount = 0;
				}

                currentAmount += amount;

                _nameAmount[name] = currentAmount;
            }

            private int GetNameAmount(string name) 
            {
				if (!_nameAmount.TryGetValue(name, out var currentAmount))
				{
					return 0;
				}

				return currentAmount;
			}

            private record UserData(string OriginalName, string MappedName);
        }
    }
}
