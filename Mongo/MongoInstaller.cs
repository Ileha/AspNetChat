using Autofac;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mongo.Common.Converter;
using Mongo.EntityFramework;

namespace Mongo;

public class MongoInstaller : AutofacInstallerBase
{
	private readonly string _connectionString;
	private readonly string _databaseName;

	public MongoInstaller(
		ContainerBuilder builder, 
		string? connectionString, 
		string? databaseName) : base(builder)
	{
		_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
		_databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
	}

	public override void Install()
	{
		var optionBuilder = new DbContextOptionsBuilder<EntityFrameworkDbContext>();
		optionBuilder.UseMongoDB(_connectionString, _databaseName);
		
		Builder
			.RegisterType<EntityFrameworkDbContext>()
			.AsSelf()
			.WithParameters([
				new TypedParameter(typeof(DbContextOptions), optionBuilder.Options)
			])
			.InstancePerLifetimeScope();

		Builder
			.RegisterType<EntityFrameworkController>()
			.AsImplementedInterfaces()
			.InstancePerLifetimeScope();
		
		Builder
			.AddFactoryTo<IIdentifiable, CancellationToken, IChatStorage, EntityFrameworkChatStorage>()
			.InstancePerLifetimeScope();

		Builder
			.AddFactory<ChatEvent2EventConverter>()
			.SingleInstance();

		Builder
			.AddFactory<IIdentifiable, Event2ChatEventConverter>()
			.SingleInstance();
	}
}