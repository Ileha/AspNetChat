using Chat.Interfaces;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mongo.Entities;
using Mongo.Interfaces;

namespace Mongo.EntityFramework;

public class EntityFrameworkController : IUserStorage, IDataBaseService
{
    private readonly IFactory<IIdentifiable, CancellationToken, IChatStorage> _chatStorageFactory;
    private readonly IDataBaseTransaction<EntityFrameworkDbContext> _dbContext;
    
    public EntityFrameworkController(
        IFactory<
            IIdentifiable, 
            CancellationToken, 
            IChatStorage> chatStorageFactory,
        IDataBaseTransaction<EntityFrameworkDbContext> dbContext)
    {
        _chatStorageFactory = chatStorageFactory ?? throw new ArgumentNullException(nameof(chatStorageFactory));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IChatParticipant> AddOrGetParticipant(IIdentifiable identifiable, IChatParticipant participant)
    {
        return await _dbContext
            .PerformTransactionAsync(async context =>
            {
                var dbUser = await context.Users.FirstOrDefaultAsync(user => user.Name == participant.Name);

                if (dbUser != null)
                    return new MongoChatParticipant(dbUser);

                var mongoUser = new User() {Id = participant.Id, Name = participant.Name};

                await context.Users.AddAsync(mongoUser);

                await context.SaveChangesAsync();

                return participant;
            });
    }

    public IChatStorage GetChatStorage(IIdentifiable chat)
    {
        return _chatStorageFactory.Create(chat, CancellationToken.None);
    }

    public async Task<bool> HasChat(IIdentifiable chat)
    {
        return await _dbContext
            .PerformTransactionAsync(async context =>
            {
                var eventsAmount = await context.UserChatEvents
                    .Where(chatEvent => chatEvent.ChatId == chat.Id)
                    .CountAsync();
                
                return eventsAmount > 0;
            });
    }
}