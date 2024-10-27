using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.DataBase.Mongo.Entities;
using AspNetChat.Extensions.DI;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AspNetChat.DataBase.Mongo
{
    public class MongoInstaller
    {
	    private readonly IServiceCollection _services;
		private readonly string _connectionString;
		private readonly string _databaseName;

		public MongoInstaller(
			IServiceCollection services, 
			string? connectionString, 
			string? databaseName) 
        {
	        _services = services ?? throw new ArgumentNullException(nameof(services));
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
			_databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
		}

        public void Install() 
        {
			BsonClassMap.RegisterClassMap<BaseUserChatEvent>();
			BsonClassMap.RegisterClassMap<UserJoined>();
			BsonClassMap.RegisterClassMap<UserSendMessage>();
			BsonClassMap.RegisterClassMap<UserDisconnected>();

			_services.BindSingletonInterfacesTo<MongoDataBaseService>(_connectionString, _databaseName);
			_services.AddFactoryTo<
				IIdentifiable,
				IMongoCollection<BaseUserChatEvent>,
				IMongoCollection<User>,
				CancellationToken,
				IChatStorage,
				MongoChatStorage>();
			_services.AddFactory<MongoChatStorage.ChatEvent2EventConverter>();
		}
    }
}
