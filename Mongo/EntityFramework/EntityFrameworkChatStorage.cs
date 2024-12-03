using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services.Storage;
using Common.Extensions.DI;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mongo.Common.Converter;
using Mongo.Interfaces;

namespace Mongo.EntityFramework;

internal class EntityFrameworkChatStorage : IChatStorage
{
    private readonly IDataBaseTransaction<EntityFrameworkDbContext> _dbContext;
    private readonly IFactory<ChatEvent2EventConverter> _chatEvent2EventConverterFactory;
    private readonly IFactory<IIdentifiable, Event2ChatEventConverter> _event2ChatEventConverterFactory;
    private readonly IIdentifiable _chat;
    private readonly CancellationToken _token;

    public EntityFrameworkChatStorage(
        IDataBaseTransaction<EntityFrameworkDbContext> dbContext,
        IFactory<ChatEvent2EventConverter> chatEvent2EventConverterFactory,
        IFactory<IIdentifiable, Event2ChatEventConverter> event2ChatEventConverterFactory,
        IIdentifiable chat,
        CancellationToken token
    )
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _chatEvent2EventConverterFactory = chatEvent2EventConverterFactory ?? throw new ArgumentNullException(nameof(chatEvent2EventConverterFactory));
        _event2ChatEventConverterFactory = event2ChatEventConverterFactory ?? throw new ArgumentNullException(nameof(event2ChatEventConverterFactory));
        _chat = chat ?? throw new ArgumentNullException(nameof(chat));
        _token = token;
    }

    public async Task<IEnumerable<IEvent>> GetChatEvents()
    {
        var (dbEvents, users) = await _dbContext
            .PerformTransactionAsync(async context =>
            {
                var dbEvents = await context.UserChatEvents.AsQueryable()
                    .Where(@event => _chat.Id.Equals(@event.ChatId))
                    .OrderBy(@event => @event.Time)
                    .ToListAsync(cancellationToken: _token);

                var allUsers = dbEvents
                    .Select(@event => @event.UserId)
                    .Distinct()
                    .ToHashSet();
        
                var users = await context.Users
                    .Where(user => allUsers.Contains(user.Id))
                    .ToDictionaryAsync(user => user.Id, user => user, cancellationToken: _token);
                
                return (dbEvents, users);
            }, _token);

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
        var convertor = _event2ChatEventConverterFactory.Create(_chat);

        @event.Accept(convertor);

        if (convertor.ChatEvent == null)
            throw new InvalidOperationException($"unable to convert from {@event.GetType()}");
        
        await _dbContext
            .PerformTransactionAsync(async context =>
            {
                await context.UserChatEvents.AddAsync(convertor.ChatEvent, _token);
                await context.SaveChangesAsync(_token);
            }, _token);
    }
}