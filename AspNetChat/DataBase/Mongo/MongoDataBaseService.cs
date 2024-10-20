using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.DataBase.Mongo.Entities;
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

		public MongoDataBaseService(string connection, string dataBaseService) 
		{
			_client = new MongoClient(connection);
			_database = _client.GetDatabase(dataBaseService);

			_chatCollections = _database.GetCollection<BaseUserChatEvent>(Constants.ChatCollectionID);
			_userCollections = _database.GetCollection<User>(Constants.UserCollectionID);

			_lifeTokenSource = new CancellationTokenSource();
			_lifeToken = _lifeTokenSource.Token;
		}

		public async Task<IChatPartisipant> AddOrGetParticipant(IIdentifiable identifiable, IChatPartisipant partisipant)
		{

			using var session = await _client.StartSessionAsync();
				
			return await session.WithTransactionAsync(async (session, token) =>
			{
				var user = await _userCollections.Find(dbUser => dbUser.Name.Equals(partisipant.Name)).FirstOrDefaultAsync(token);

				if (user != null)
					return new MongoChatParticipant(user);

				var mongoUser = new User() { Id = partisipant.Id, Name = partisipant.Name };

				await _userCollections.InsertOneAsync(mongoUser, options: null, token);

				return partisipant;
			}, cancellationToken: _lifeToken);
		}

		public IChatStorage GetChatStorage(IIdentifiable chat)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			_lifeTokenSource.Cancel();
			_lifeTokenSource.Dispose();
			_client.Dispose();
		}
	}
}
