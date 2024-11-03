using Chat.Interfaces;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mongo.Entities;

namespace Mongo.EntityFramework;

public class EntityFrameworkController : IUserStorage, IDataBaseService
{
    private readonly IFactory<IIdentifiable, CancellationToken, IChatStorage> _chatStorageFactory;
    private readonly EntityFrameworkDbContext _dbContext;
    
    public EntityFrameworkController(
        IFactory<
            IIdentifiable, 
            CancellationToken, 
            IChatStorage> chatStorageFactory,
        EntityFrameworkDbContext dbContext)
    {
        _chatStorageFactory = chatStorageFactory ?? throw new ArgumentNullException(nameof(chatStorageFactory));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IChatParticipant> AddOrGetParticipant(IIdentifiable identifiable, IChatParticipant participant)
    {
        var dbUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Name == participant.Name);
        
        if (dbUser != null)
            return new MongoChatParticipant(dbUser);
        
        var mongoUser = new User() { Id = participant.Id, Name = participant.Name };

        await _dbContext.Users.AddAsync(mongoUser);

        await _dbContext.SaveChangesAsync();

        return participant;
    }

    public IChatStorage GetChatStorage(IIdentifiable chat)
    {
        return _chatStorageFactory.Create(chat, CancellationToken.None);
    }

    public async Task<bool> HasChat(IIdentifiable chat)
    {
        var eventsAmount = await _dbContext.UserChatEvents
            .Where(chatEvent => chatEvent.UserId == chat.Id)
            .CountAsync();

        return eventsAmount > 0;
    }
}