using Chat.Entities.ChatModel.Events;
using Chat.Entities.EventVisitor;
using Chat.Extensions.Comparers;
using Chat.Interfaces;
using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using static Chat.Interfaces.IChat;

namespace Chat.Entities.ChatModel;

public class ChatModel : IChat
{
	public Guid Id => _chatParams.Guid;
	
	private readonly IFactory<ChatUsersExtractor> _usersExtractorFactory;
	private readonly IFactory<CurrentParticipantsCountExtractor> _usersCountsExtractorFactory;
	private readonly IFactory<UserConnected.NewParams, UserConnected> _userConnectedFactory;
	private readonly IFactory<UserSendMessage.NewParams, UserSendMessage> _userSendMessageFactory;
	private readonly IFactory<UserDisconnected.NewParams, UserDisconnected> _userDisconnected;
	private readonly ChatParams _chatParams;
	private readonly IMessageConsumerService _messageConsumerService;
	private readonly IUserStorage _userStorage;

	public ChatModel(
		IFactory<ChatUsersExtractor> usersExtractorFactory,
		IFactory<CurrentParticipantsCountExtractor> usersCountsExtractorFactory,
		IFactory<UserConnected.NewParams, UserConnected> userConnectedFactory,
		IFactory<UserSendMessage.NewParams, UserSendMessage> userSendMessageFactory,
		IFactory<UserDisconnected.NewParams, UserDisconnected> userDisconnected,
		ChatParams chatParams,
		IMessageConsumerService messageConsumerService, 
		IUserStorage userStorage)
	{
		_usersExtractorFactory = usersExtractorFactory ?? throw new ArgumentNullException(nameof(usersExtractorFactory));
		_usersCountsExtractorFactory = usersCountsExtractorFactory ?? throw new ArgumentNullException(nameof(usersCountsExtractorFactory));
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

	public async Task<IChatParticipant> JoinParticipant(IChatParticipant participant)
	{
		var eventTime = GetTime();
		
		var realParticipant = await _userStorage.AddOrGetParticipant(participant, participant);

		var @event = _userConnectedFactory.Create(
			new UserConnected.NewParams(
				realParticipant.Id, 
				realParticipant.Name,
				eventTime));
		
		await _chatParams.ChatStorage.AddEvent(@event);

		await PostEvents();

		return realParticipant;
	}

	public async Task SendMessage(IIdentifiable participant, string message)
	{
		var @event = _userSendMessageFactory.Create(new UserSendMessage.NewParams(participant.Id, message, GetTime()));
		
		var extractor = await ApplyVisitor2ChatEvents(_usersCountsExtractorFactory.Create());

		if (!extractor.ParticipantsCount.TryGetValue(participant, out var participantsCount) || participantsCount < 1)
			throw new InvalidOperationException($"Participant with {participant.Id} is not exist or not joined to chat");
		
		await _chatParams.ChatStorage.AddEvent(@event);

		await PostEvents();
	}

	public async Task DisconnectedParticipant(IIdentifiable participant)
	{
		var @event = _userDisconnected.Create(new UserDisconnected.NewParams(participant.Id, GetTime()));
		
		var extractor = await ApplyVisitor2ChatEvents(_usersCountsExtractorFactory.Create());
		
		if (!extractor.ParticipantsCount.TryGetValue(participant, out var participantsCount) || participantsCount < 1)
			throw new InvalidOperationException($"Participant with {participant.Id} is not exist or not joined to chat");
		
		await _chatParams.ChatStorage.AddEvent(@event);

		await PostEvents();
	}

	public async Task<bool> HasParticipant(IIdentifiable participant)
	{
		var extractor = await ApplyVisitor2ChatEvents(_usersExtractorFactory.Create());

		var users = extractor.ToHashSet(new IdentifiableEqualityComparer());

		return users.Contains(participant);
	}

	private async Task<T> ApplyVisitor2ChatEvents<T>(T eventVisitor)
		where T : IEventVisitor
	{
		var events = await _chatParams.ChatStorage.GetChatEvents();

		foreach (var @event in events) 
			@event.Accept(eventVisitor);

		return eventVisitor;
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