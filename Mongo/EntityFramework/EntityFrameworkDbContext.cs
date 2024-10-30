using Microsoft.EntityFrameworkCore;
using Mongo.Entities;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Mongo.EntityFramework;

public class EntityFrameworkDbContext : DbContext
{
    public DbSet<User> Users { get; init; }
    public DbSet<BaseUserChatEvent> UserChatEvents { get; init; }

    public EntityFrameworkDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
			
        modelBuilder.Entity<User>().ToCollection("EFUsers");
        
        // modelBuilder.Entity<User>(builder => 
        //     builder.Property(entity => entity.Id).HasElementName("_id")
        // );
        
        modelBuilder.Entity<BaseUserChatEvent>().ToCollection("EFBaseUserChatEvent");
        modelBuilder.Entity<UserJoined>().ToCollection("EFUserJoined");
        modelBuilder.Entity<UserSendMessage>().ToCollection("EFUserSendMessage");
        modelBuilder.Entity<UserDisconnected>().ToCollection("EFUserDisconnected");
    }
}