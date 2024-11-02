﻿using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Entities;
using Mongo.EntityFramework;
using MongoDB.Bson.Serialization;

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
	        Services.AddDbContext<EntityFrameworkDbContext>(
		        options => options.UseMongoDB(_connectionString, _databaseName), 
		        ServiceLifetime.Singleton, 
		        ServiceLifetime.Singleton);
		       
	        // Services.AddDbContext<EntityFrameworkDbContext>(options => options.UseMongoDb())

	        Services.BindSingletonInterfacesTo<EntityFrameworkController>();
	        
	        Services.AddFactoryTo<
		        IIdentifiable, 
		        CancellationToken, 
		        IChatStorage, 
		        EntityFrameworkChatStorage>();
	        
			// BsonClassMap.RegisterClassMap<BaseUserChatEvent>();
			// BsonClassMap.RegisterClassMap<UserJoined>();
			// BsonClassMap.RegisterClassMap<UserSendMessage>();
			// BsonClassMap.RegisterClassMap<UserDisconnected>();
			//
			// Services.BindSingletonInterfacesTo<MongoDataBaseService>(_connectionString, _databaseName);
			// Services.AddFactoryTo<
			// 	IIdentifiable,
			// 	IMongoCollection<BaseUserChatEvent>,
			// 	IMongoCollection<User>,
			// 	CancellationToken,
			// 	IChatStorage,
			// 	MongoChatStorage>();
			Services.AddFactory<MongoChatStorage.ChatEvent2EventConverter>();
		}
    }
}
