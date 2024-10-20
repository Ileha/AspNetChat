using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Core.Interfaces.Services.Storage
{
    public interface IChatStorage
    {
        Task<IEnumerable<IEvent>> GetChatEvents();
        Task AddEvent(IEvent @event);
    }
}
