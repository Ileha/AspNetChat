using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mongo.Entities;

namespace Mongo.EntityFramework;

internal class EntityFrameworkChatStorage : IChatStorage
{
    private readonly EntityFrameworkDbContext _dbContext;
    private readonly IFactory<MongoChatStorage.ChatEvent2EventConverter> _chatEvent2EventConverterFactory;
    private readonly IIdentifiable _chat;
    private readonly CancellationToken _token;

    public EntityFrameworkChatStorage(
        EntityFrameworkDbContext dbContext,
        IFactory<MongoChatStorage.ChatEvent2EventConverter> chatEvent2EventConverterFactory,
        IIdentifiable chat,
        CancellationToken token
    )
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _chatEvent2EventConverterFactory = chatEvent2EventConverterFactory ?? throw new ArgumentNullException(nameof(chatEvent2EventConverterFactory));
        _chat = chat ?? throw new ArgumentNullException(nameof(chat));
        _token = token;
    }

    public async Task<IEnumerable<IEvent>> GetChatEvents()
    {
        var dbEvents = await _dbContext.UserChatEvents.AsQueryable()
            .Where(@event => _chat.Id.Equals(@event.ChatId))
            .OrderBy(@event => @event.Time)
            .ToListAsync(cancellationToken: _token);

        var allUsers = dbEvents
            .Select(@event => @event.UserId)
            .Distinct()
            .ToHashSet();
        
        var users = await _dbContext.Users
            .Where(user => allUsers.Contains(user.Id))
            .ToDictionaryAsync(user => user.Id, user => user, cancellationToken: _token);

        var converter = _chatEvent2EventConverterFactory.Create();

        return dbEvents
            .Select(
                item =>
                {
                    if (!users.TryGetValue(item.UserId, out var user))
                        throw new InvalidOperationException($"unable to find user with id {item.UserId}");
                    
                    converter.SetUser(user);
                    item.Accept(converter);

                    if (converter.Event == null)
                        throw new InvalidOperationException($"unable to convert event with type {item.GetType()}");

                    return converter.Event;
                });
    }

    public async Task AddEvent(IEvent @event)
    {
        var convertor = new MongoChatStorage.Event2ChatEventConverter(_chat);

        @event.Accept(convertor);

        if (convertor.ChatEvent == null)
            throw new InvalidOperationException($"unable to convert from {@event.GetType()}");

        await _dbContext.UserChatEvents.AddAsync(convertor.ChatEvent, _token);
        
        await _dbContext.SaveChangesAsync(_token);
    }
}