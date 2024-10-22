using AspNetChat.Core.Entities.ChatModel.Events;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.Extensions.Comparers;
using System.Collections.Concurrent;

namespace AspNetChat.Core.Entities.Model
{
    public class ChatModel : IChat
    {
        public Guid Id { get; }

        private readonly ConcurrentDictionary<IIdentifiable, byte> _participants = new ConcurrentDictionary<IIdentifiable, byte>(new IdentifiableEqualityComparer());
		private readonly IMessageConsumerService _messageConsumerService;
		private readonly IChatStorage _chatStorage;
		private readonly IUserStorage _userStorage;

		public ChatModel(
            Guid guid, 
            IMessageConsumerService messageConsumerService, 
            IChatStorage chatStorage,
            IUserStorage userStorage)
        {
            Id = guid;
			_messageConsumerService = messageConsumerService ?? throw new ArgumentNullException(nameof(messageConsumerService));
			_chatStorage = chatStorage ?? throw new ArgumentNullException(nameof(chatStorage));
			_userStorage = userStorage ?? throw new ArgumentNullException(nameof(userStorage));
		}

        public async Task<IReadOnlyList<IEvent>> GetChatMessageList()
        {
            return (await _chatStorage.GetChatEvents()).ToArray();
        }

        public async Task JoinParticipant(IChatPartisipant partisipant)
        {
            var realParticipant = await _userStorage.AddOrGetParticipant(partisipant, partisipant);

            if (!_participants.TryAdd(realParticipant, 0))
				throw new InvalidOperationException($"chat already has participant with {realParticipant.Id}");

            await _chatStorage.AddEvent(new UserConnected(realParticipant.Name, realParticipant.Id, GetTime()));

            PostEvents();
        }

        public async Task SendMessage(IIdentifiable partisipant, string message)
        {
            if (!_participants.ContainsKey(partisipant))
				throw new InvalidOperationException($"chat don't have participant with {partisipant.Id}");

            await _chatStorage.AddEvent(new UserSendMessage(partisipant.Id, message, GetTime()));

            PostEvents();
		}

		public async Task DisconnectedParticipant(IIdentifiable partisipant)
		{
            if (!_participants.TryRemove(partisipant, out _))
				throw new InvalidOperationException($"chat don't have participant with {partisipant.Id}");

            await _chatStorage.AddEvent(new UserDisconnected(partisipant.Id, GetTime()));

			PostEvents();
		}

		public bool HasPartisipant(IIdentifiable partisipant)
		{
			return _participants.ContainsKey(partisipant);
		}

		private DateTime GetTime()
        {
            return DateTime.UtcNow;
        }

        private Task PostEvents()
        {
            return Task.Run(async () =>
            {
                var messages = await GetChatMessageList();

				_messageConsumerService.ConsumeMessage(this, messages);
			});
        }
	}
}
