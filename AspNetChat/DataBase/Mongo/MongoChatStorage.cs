using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.DataBase.Mongo.Entities;
using AspNetChat.DataBase.Mongo.Inerfaces;
using Common.Extensions.DI;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using UserConnectedMongo = AspNetChat.DataBase.Mongo.Entities.UserJoined;
using UserDisconnectedMongo = AspNetChat.DataBase.Mongo.Entities.UserDisconnected;
using UserSendMessageMongo = AspNetChat.DataBase.Mongo.Entities.UserSendMessage;
using UserConnected = AspNetChat.Core.Entities.ChatModel.Events.UserConnected;
using UserDisconnected = AspNetChat.Core.Entities.ChatModel.Events.UserDisconnected;
using UserSendMessage = AspNetChat.Core.Entities.ChatModel.Events.UserSendMessage;

namespace AspNetChat.DataBase.Mongo
{
	internal class MongoChatStorage : IChatStorage
	{
		private readonly IFactory<ChatEvent2EventConverter> _chatEvent2EventConverterFactory;
		private readonly IIdentifiable _chat;
		private readonly IMongoCollection<BaseUserChatEvent> _chatCollection;
		private readonly IMongoCollection<User> _userCollection;
		private readonly CancellationToken _token;

		public MongoChatStorage(
			IFactory<ChatEvent2EventConverter> chatEvent2EventConverterFactory,
			IIdentifiable chat,
			IMongoCollection<BaseUserChatEvent> chatCollection,
			IMongoCollection<User> userCollection, 
			CancellationToken token) 
		{
			_chatEvent2EventConverterFactory = chatEvent2EventConverterFactory ?? throw new ArgumentNullException(nameof(chatEvent2EventConverterFactory));
			_chat = chat ?? throw new ArgumentNullException(nameof(chat));
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
				.ToListAsync(cancellationToken: _token);

			var converter = _chatEvent2EventConverterFactory.Create();

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

		public class ChatEvent2EventConverter : IUserChatEventVisitor
		{
			private readonly IFactory<UserConnected.Params, UserConnected> _userConnectedFactory;
			private readonly IFactory<UserSendMessage.Params,UserSendMessage> _userSendMessageFactory;
			private readonly IFactory<UserDisconnected.Params, UserDisconnected> _userDisconnected;
			private User? _eventUser;
			public IEvent? Event { get; private set; }

			public ChatEvent2EventConverter(
				IFactory<UserConnected.Params, UserConnected> userConnectedFactory,
				IFactory<UserSendMessage.Params, UserSendMessage> userSendMessageFactory,
				IFactory<UserDisconnected.Params, UserDisconnected> userDisconnected
				)
			{
				_userConnectedFactory = userConnectedFactory ?? throw new ArgumentNullException(nameof(userConnectedFactory));
				_userSendMessageFactory = userSendMessageFactory ?? throw new ArgumentNullException(nameof(userSendMessageFactory));
				_userDisconnected = userDisconnected ?? throw new ArgumentNullException(nameof(userDisconnected));
			}

			public void SetUser(User eventUser) 
			{
				_eventUser = eventUser ?? throw new ArgumentNullException(nameof(eventUser));
			}

			public void Visit(UserConnectedMongo joined)
			{
				CheckIDEquality(joined);

				Event = _userConnectedFactory.Create(
					new UserConnected.Params(
						joined.EventId, 
						joined.UserId, 
						_eventUser!.Name, 
						joined.Time));
			}

			public void Visit(UserSendMessageMongo sendMessage)
			{
				CheckIDEquality(sendMessage);
				
				Event = _userSendMessageFactory.Create(
					new UserSendMessage.Params(
						sendMessage.EventId, 
						sendMessage.UserId, 
						sendMessage.Message,
						sendMessage.Time));
			}

			public void Visit(UserDisconnectedMongo disconnected)
			{
				CheckIDEquality(disconnected);

				Event = _userDisconnected.Create(new UserDisconnected.Params(disconnected.EventId, disconnected.UserId, disconnected.Time));
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
				ChatEvent = new UserConnectedMongo()
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
				ChatEvent = new UserSendMessageMongo()
				{
					ChatId = _chat.Id,
					UserId = userSendMessage.User.Id,
					EventType = UserEventType.Message,
					Time = userSendMessage.DateTime,
					EventId = userSendMessage.Id,
					Message = userSendMessage.Message,
				};
			}

			public void Visit(IUserDisconnected userDisconnected)
			{
				ChatEvent = new UserDisconnectedMongo()
				{
					ChatId = _chat.Id,
					UserId = userDisconnected.User.Id,
					EventType = UserEventType.Disconnected,
					Time = userDisconnected.DateTime,
					EventId = userDisconnected.Id,
				};
			}
		}

		private record struct DBEvent(BaseUserChatEvent Event, User User);
	}
}
