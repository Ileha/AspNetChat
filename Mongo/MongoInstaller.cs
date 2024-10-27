using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Mongo
{
    public class MongoInstaller : InstallerBase
    {
		private readonly string _connectionString;
		private readonly string _databaseName;

		public MongoInstaller(
			IServiceCollection services, 
			string? connectionString, 
			string? databaseName) : base(services)
        {
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
			_databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
		}

        public override void Install() 
        {
			BsonClassMap.RegisterClassMap<BaseUserChatEvent>();
			BsonClassMap.RegisterClassMap<UserJoined>();
			BsonClassMap.RegisterClassMap<UserSendMessage>();
			BsonClassMap.RegisterClassMap<UserDisconnected>();

			Services.BindSingletonInterfacesTo<MongoDataBaseService>(_connectionString, _databaseName);
			Services.AddFactoryTo<
				IIdentifiable,
				IMongoCollection<BaseUserChatEvent>,
				IMongoCollection<User>,
				CancellationToken,
				IChatStorage,
				MongoChatStorage>();
			Services.AddFactory<MongoChatStorage.ChatEvent2EventConverter>();
		}
    }
}
