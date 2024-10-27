using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.DataBase.Mongo.Entities;
using AspNetChat.Extensions.DI;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AspNetChat.DataBase.Mongo
{
	public class MongoDataBaseService : IDataBaseService, IUserStorage, IDisposable
	{
		private readonly IMongoClient _client;
		private readonly IMongoDatabase _database;
		private readonly IMongoCollection<BaseUserChatEvent> _chatCollections;
		private readonly IMongoCollection<User> _userCollections;
		
		private readonly CancellationTokenSource _lifeTokenSource;
		private readonly CancellationToken _lifeToken;
		private readonly IFactory<IIdentifiable, IMongoCollection<BaseUserChatEvent>, IMongoCollection<User>, CancellationToken, IChatStorage> _chatStorageFactory;

		public MongoDataBaseService(
			string connection, 
			string dataBaseService,
			IFactory<
				IIdentifiable,
				IMongoCollection<BaseUserChatEvent>,
				IMongoCollection<User>,
				CancellationToken,
				IChatStorage> chatStorageFactory) 
		{
			_chatStorageFactory = chatStorageFactory ?? throw new ArgumentNullException(nameof(chatStorageFactory));
			_client = new MongoClient(connection);
			_database = _client.GetDatabase(dataBaseService);

			_chatCollections = _database.GetCollection<BaseUserChatEvent>(Constants.ChatCollectionID);
			_userCollections = _database.GetCollection<User>(Constants.UserCollectionID);

			_lifeTokenSource = new CancellationTokenSource();
			_lifeToken = _lifeTokenSource.Token;
		}

		public async Task<IChatPartisipant> AddOrGetParticipant(IIdentifiable identifiable, IChatPartisipant partisipant)
		{
			var mongoUser = new User() { Id = partisipant.Id, Name = partisipant.Name };

			var updateResult = await _userCollections.UpdateOneAsync(
				dbUser => dbUser.Name.Equals(partisipant.Name),
				new BsonDocument("$setOnInsert", mongoUser.ToBsonDocument()),
				new UpdateOptions() { IsUpsert = true }, cancellationToken: _lifeToken);

			if (!updateResult.IsAcknowledged)
				throw new InvalidOperationException($"failed to update user");

			var user = await _userCollections
				.AsQueryable()
				.Where(dbUser => dbUser.Name.Equals(partisipant.Name))
				.FirstAsync(cancellationToken: _lifeToken);

			return new MongoChatParticipant(user);
		}

		public IChatStorage GetChatStorage(IIdentifiable chat)
		{
			return _chatStorageFactory.Create(chat, _chatCollections, _userCollections, _lifeToken);
		}

		public void Dispose()
		{
			_lifeTokenSource.Cancel();
			_lifeTokenSource.Dispose();
			_client.Dispose();
		}
	}
}
