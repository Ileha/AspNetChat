using AspNetChat.Core.Entities.ChatModel.Events;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.DataBase.Mongo.Entities;
using AspNetChat.DataBase.Mongo.Inerfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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
			var dbEvents = await _chatCollection
				.AsQueryable()
				.Where(@event => _chat.Id.Equals(@event.ChatId))
				.OrderBy(@event => @event.Time)
				.Join(
					_userCollection.AsQueryable(),
					@event => @event.UserId,
					user => user.Id,
					(@event, user) => new DBEvent(@event, user)
				)
				.ToListAsync();

			var converter = new ChatEvent2EventConverter();

			return dbEvents
				.Select(
					item =>
					{
						converter.SetUser(item.User);
						item.Event.Accept(converter);

						if (converter.Event == null)
							throw new InvalidOperationException($"unable to convert event with type {item.GetType()}");

						return converter.Event;
					});
		}

		private class ChatEvent2EventConverter : IUserChatEventVisitor
		{
			private User? _eventUser;
			public IEvent? Event { get; private set; }

			public void SetUser(User eventUser) 
			{
				_eventUser = eventUser ?? throw new ArgumentNullException(nameof(eventUser));
			}

			public void Visit(UserJoined joined)
			{
				CheckIDEquality(joined);

				Event = new UserConnected(_eventUser!.Name, _eventUser.Id, joined.Time);
			}

			public void Visit(UserSendMessage sendMessage)
			{
				CheckIDEquality(sendMessage);

				Event = new Core.Entities.ChatModel.Events.UserSendMessage(sendMessage.EventId, sendMessage.Message, sendMessage.Time);
			}

			public void Visit(UserDisconnected disconnected)
			{
				CheckIDEquality(disconnected);

				Event = new Core.Entities.ChatModel.Events.UserDisconnected(disconnected.EventId, disconnected.Time);
			}

			private void CheckIDEquality(BaseUserChatEvent @event) 
			{
				if (_eventUser == null || !@event.UserId.Equals(_eventUser.Id))
					throw new InvalidOperationException("event and user id's not equal");
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

		private record struct DBEvent(BaseUserChatEvent Event, User User);
	}
}
