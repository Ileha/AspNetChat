using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Common.Converter;
using Mongo.EntityFramework;

namespace Mongo;

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
			options => options.UseMongoDB(_connectionString, _databaseName));

		Services.Scan(scan =>
		{
			scan
				.FromAssemblyOf<MongoInstaller>()
				.AddClasses(@class => @class.AssignableTo<EntityFrameworkController>())
				.AsImplementedInterfaces()
				.WithScopedLifetime();
		});
	        
		Services.AddFactoryTo<
			IIdentifiable, 
			CancellationToken, 
			IChatStorage, 
			EntityFrameworkChatStorage>();
	        
		Services.AddFactory<ChatEvent2EventConverter>();
		Services.AddFactory<IIdentifiable, Event2ChatEventConverter>();
	}
}