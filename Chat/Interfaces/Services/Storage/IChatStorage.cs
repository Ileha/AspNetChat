using Chat.Interfaces.ChatEvents;

namespace Chat.Interfaces.Services.Storage
{
    public interface IChatStorage
    {
        Task<IEnumerable<IEvent>> GetChatEvents();
        Task AddEvent(IEvent @event);
    }
}
