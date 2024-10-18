using AspNetChat.Core.Entities.ChatModel.Events;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services;
using AspNetChat.Extensions.Comparers;
using System.Collections.Concurrent;

namespace AspNetChat.Core.Entities.Model
{
    public class ChatModel : IChat
    {
        public Guid Id { get; }

        private readonly object _eventsLocker = new object();
        private readonly SortedSet<IEvent> _events = new SortedSet<IEvent>(new EventsComparer());
        private readonly ConcurrentDictionary<IIdentifiable, byte> _participants = new ConcurrentDictionary<IIdentifiable, byte>(new IdentifiableEqualityComparer());
		private readonly IMessageConsumerService _messageConsumerService;

		public ChatModel(Guid guid, IMessageConsumerService messageConsumerService)
        {
            Id = guid;
			_messageConsumerService = messageConsumerService ?? throw new ArgumentNullException(nameof(messageConsumerService));
		}

        public IReadOnlyList<IEvent> GetChatMessageList()
        {
            lock (_eventsLocker)
            {
                return _events.ToArray();
			}
        }

        public void JoinParticipant(IChatPartisipant partisipant)
        {
            if (!_participants.TryAdd(partisipant, 0))
				throw new InvalidOperationException($"chat already has participant with {partisipant.Id}");

			lock (_eventsLocker)
            {
                _events.Add(new UserConnected(partisipant.Name, partisipant.Id, GetTime()));
            }

            PostEvents();
        }

        public void SendMessage(IIdentifiable partisipant, string message)
        {
            if (!_participants.ContainsKey(partisipant))
				throw new InvalidOperationException($"chat don't have participant with {partisipant.Id}");

			lock (_eventsLocker)
            {
                _events.Add(new UserSendMessage(partisipant.Id, message, GetTime()));
            }

            PostEvents();
		}

		public void DisconnectedParticipant(IIdentifiable partisipant)
		{
            if (!_participants.TryRemove(partisipant, out _))
				throw new InvalidOperationException($"chat don't have participant with {partisipant.Id}");

			lock (_eventsLocker)
			{
				_events.Add(new UserDisconnected(partisipant.Id, GetTime()));
			}

			PostEvents();
		}

		private DateTime GetTime()
        {
            return DateTime.UtcNow;
        }

        private Task PostEvents()
        {
            return Task.Run(() =>
            {

                var messages = GetChatMessageList();

				_messageConsumerService.ConsumeMessage(this, messages);


			});
        }

		public bool HasPartisipant(IIdentifiable partisipant)
		{
            return _participants.ContainsKey(partisipant);
		}
    }
}
