using System.Collections.Concurrent;
using Chat.Entities.ChatModel.Events;
using Chat.Extensions.Comparers;
using Chat.Interfaces;
using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using static Chat.Interfaces.IChat;

namespace Chat.Entities.ChatModel
{
    public class ChatModel : IChat
    {
        public Guid Id => _chatParams.Guid;

        private readonly ConcurrentDictionary<IIdentifiable, byte> _participants = new(new IdentifiableEqualityComparer());
        private readonly IFactory<UserConnected.NewParams, UserConnected> _userConnectedFactory;
        private readonly IFactory<UserSendMessage.NewParams, UserSendMessage> _userSendMessageFactory;
        private readonly IFactory<UserDisconnected.NewParams, UserDisconnected> _userDisconnected;
        private readonly ChatParams _chatParams;
		private readonly IMessageConsumerService _messageConsumerService;
		private readonly IUserStorage _userStorage;

		public ChatModel(
			IFactory<UserConnected.NewParams, UserConnected> userConnectedFactory,
			IFactory<UserSendMessage.NewParams, UserSendMessage> userSendMessageFactory,
			IFactory<UserDisconnected.NewParams, UserDisconnected> userDisconnected,
			ChatParams chatParams,
            IMessageConsumerService messageConsumerService, 
            IUserStorage userStorage)
        {
	        _userConnectedFactory = userConnectedFactory ?? throw new ArgumentNullException(nameof(userConnectedFactory));
	        _userSendMessageFactory = userSendMessageFactory ?? throw new ArgumentNullException(nameof(userSendMessageFactory));
	        _userDisconnected = userDisconnected ?? throw new ArgumentNullException(nameof(userDisconnected));
	        _chatParams = chatParams ?? throw new ArgumentNullException(nameof(chatParams));
			_messageConsumerService = messageConsumerService ?? throw new ArgumentNullException(nameof(messageConsumerService));
			_userStorage = userStorage ?? throw new ArgumentNullException(nameof(userStorage));
		}

        public async Task<IReadOnlyList<IEvent>> GetChatMessageList()
        {
            return (await _chatParams.ChatStorage.GetChatEvents()).ToArray();
        }

        public async Task<IChatPartisipant> JoinParticipant(IChatPartisipant partisipant)
        {
            var realParticipant = await _userStorage.AddOrGetParticipant(partisipant, partisipant);

            if (!_participants.TryAdd(realParticipant, 0))
				throw new InvalidOperationException($"chat already has participant with {realParticipant.Id}");

            var @event = _userConnectedFactory.Create(
	            new UserConnected.NewParams(
		            realParticipant.Id, 
		            realParticipant.Name,
		            GetTime()));
            await _chatParams.ChatStorage.AddEvent(@event);

            PostEvents();

            return realParticipant;
        }

        public async Task SendMessage(IIdentifiable partisipant, string message)
        {
            if (!_participants.ContainsKey(partisipant))
				throw new InvalidOperationException($"chat don't have participant with {partisipant.Id}");

            var @event = _userSendMessageFactory.Create(new UserSendMessage.NewParams(partisipant.Id, message, GetTime()));
            await _chatParams.ChatStorage.AddEvent(@event);

            PostEvents();
		}

		public async Task DisconnectedParticipant(IIdentifiable partisipant)
		{
            if (!_participants.TryRemove(partisipant, out _))
				throw new InvalidOperationException($"chat don't have participant with {partisipant.Id}");

            var @event = _userDisconnected.Create(new UserDisconnected.NewParams(partisipant.Id, GetTime()));
            await _chatParams.ChatStorage.AddEvent(@event);

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
