using Chat.Interfaces;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mongo.Entities;

namespace Mongo.EntityFramework;

public class EntityFrameworkController : IUserStorage, IDataBaseService, IDisposable
{
    private readonly IFactory<IIdentifiable, CancellationToken, IChatStorage> _chatStorageFactory;
    private readonly EntityFrameworkDbContext _dbContext;
    private readonly CancellationTokenSource _lifeTokenSource;
    private readonly CancellationToken _lifeToken;
    
    public EntityFrameworkController(
        IFactory<
            IIdentifiable, 
            CancellationToken, 
            IChatStorage> chatStorageFactory,
        EntityFrameworkDbContext dbContext)
    {
        _chatStorageFactory = chatStorageFactory ?? throw new ArgumentNullException(nameof(chatStorageFactory));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _lifeTokenSource = new CancellationTokenSource();
        _lifeToken = _lifeTokenSource.Token;
    }

    public async Task<IChatPartisipant> AddOrGetParticipant(IIdentifiable identifiable, IChatPartisipant partisipant)
    {
        var dbUser = await _dbContext.Users.FirstOrDefaultAsync(user => user.Name == partisipant.Name, cancellationToken: _lifeToken);
        
        if (dbUser != null)
            return new MongoChatParticipant(dbUser);
        
        var mongoUser = new User() { Id = partisipant.Id, Name = partisipant.Name };

        await _dbContext.Users.AddAsync(mongoUser, _lifeToken);

        await _dbContext.SaveChangesAsync(_lifeToken);

        return partisipant;
    }

    public IChatStorage GetChatStorage(IIdentifiable chat)
    {
        return _chatStorageFactory.Create(chat, _lifeToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _lifeTokenSource.Cancel();
        _lifeTokenSource.Dispose();
    }
}