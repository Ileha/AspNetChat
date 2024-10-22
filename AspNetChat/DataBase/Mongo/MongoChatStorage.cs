using AspNetChat.Core.Entities.ChatModel.Events;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.DataBase.Mongo.Entities;
using AspNetChat.DataBase.Mongo.Inerfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TaskExtensions = AspNetChat.Extensions.TaskExtensions;
using UserDisconnected = AspNetChat.DataBase.Mongo.Entities.UserDisconnected;
using UserSendMessage = AspNetChat.DataBase.Mongo.Entities.UserSendMessage;

namespace AspNetChat.DataBase.Mongo
{
	internal class MongoChatStorage : IChatStorage
	{
		private readonly IIdentifiable _chat;
		private readonly IMongoClient _client;
		private readonly IMongoCollection<BaseUserChatEvent> _chatCollection;
		private readonly IMongoCollection<User> _userCollection;
		private readonly CancellationToken _token;

		public MongoChatStorage(
			IIdentifiable chat,
			IMongoClient client, 
			IMongoCollection<BaseUserChatEvent> chatCollection,
			IMongoCollection<User> userCollection, 
			CancellationToken token) 
		{
			_chat = chat ?? throw new ArgumentNullException(nameof(chat));
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_chatCollection = chatCollection ?? throw new ArgumentNullException(nameof(chatCollection));
			_userCollection = userCollection ?? throw new ArgumentNullException(nameof(userCollection));
			_token = token;
		}

		public async Task AddEvent(IEvent @event)
		{
			var convertor = new Event2ChatEventConverter(_chat);

			@event.Accept(convertor);

			if (convertor.ChatEvent == null)
				throw new InvalidOperationException($"unable to convert from {@event.GetType()}");

			await _chatCollection.InsertOneAsync(convertor.ChatEvent, cancellationToken: _token);
		}

		public async Task<IEnumerable<IEvent>> GetChatEvents()
		{
			using var session = await _client.StartSessionAsync();

			var dbEvents = await session.WithTransactionAsync(async (session, token) =>
			{
				var data = await TaskExtensions.WhenAll(
					_chatCollection
						.AsQueryable()
						.Where(@event => _chat.Id.Equals(@event.ChatId))
						.OrderBy(@event => @event.Time)
						.ToListAsync(token),
					_chatCollection
						.AsQueryable()
						.Where(@event => _chat.Id.Equals(@event.ChatId) && @event is UserJoined)
						.Select(@event => @event.UserId)
						.Distinct()
						.Join(
							_userCollection.AsQueryable(),
							eventId => eventId,
							user => user.Id,
							(_, user) => user)
						.ToListAsync(token)
					);

				var id2User = data.Item2.ToDictionary(data => data.Id);

				return (id2User, data.Item1);

			}, cancellationToken: _token);

			var converter = new ChatEvent2EventConverter(dbEvents.id2User);

			return dbEvents.Item2
				.Select(
					item =>
					{
						item.Accept(converter);

						if (converter.Event == null)
							throw new InvalidOperationException($"unable to convert event with type {item.GetType()}");

						return converter.Event;
					});
		}

		private class ChatEvent2EventConverter : IUserChatEventVisitor
		{
			private readonly IReadOnlyDictionary<Guid, User> _id2UserData;

			public IEvent? Event { get; private set; }

			public ChatEvent2EventConverter(IReadOnlyDictionary<Guid, User> id2UserData) 
			{
				_id2UserData = id2UserData ?? throw new ArgumentNullException(nameof(id2UserData));
			}

			public void Visit(UserJoined joined)
			{
				if (!_id2UserData.TryGetValue(joined.UserId, out var userData))
					throw new InvalidOperationException($"unable to find user with id {joined.UserId}");

				Event = new UserConnected(userData.Name, userData.Id, joined.Time);
			}

			public void Visit(UserSendMessage sendMessage)
			{
				Event = new Core.Entities.ChatModel.Events.UserSendMessage(sendMessage.EventId, sendMessage.Message, sendMessage.Time);
			}

			public void Visit(UserDisconnected disconnected)
			{
				Event = new Core.Entities.ChatModel.Events.UserDisconnected(disconnected.EventId, disconnected.Time);
			}
		}

		private class Event2ChatEventConverter : IEventVisitor
		{
			private readonly IIdentifiable _chat;

			public BaseUserChatEvent? ChatEvent { get; private set; }

			public Event2ChatEventConverter(IIdentifiable chat) 
			{
				_chat = chat ?? throw new ArgumentNullException(nameof(chat));
			}

			public void Visit(IUserConnected userConnected)
			{
				ChatEvent = new UserJoined()
				{
					ChatId = _chat.Id,
					UserId = userConnected.User.Id,
					EventType = UserEventType.Joined,
					Time = userConnected.DateTime,
					EventId = userConnected.Id,
				};
			}

			public void Visit(IUserSendMessage userSendMessage)
			{
				ChatEvent = new UserSendMessage()
				{
					ChatId = _chat.Id,
					UserId = userSendMessage.User.Id,
					EventType = UserEventType.Joined,
					Time = userSendMessage.DateTime,
					EventId = userSendMessage.Id,
					Message = userSendMessage.Message,
				};
			}

			public void Visit(IUserDisconnected userDisconnected)
			{
				ChatEvent = new UserDisconnected()
				{
					ChatId = _chat.Id,
					UserId = userDisconnected.User.Id,
					EventType = UserEventType.Joined,
					Time = userDisconnected.DateTime,
					EventId = userDisconnected.Id,
				};
			}
		}
	}
}
