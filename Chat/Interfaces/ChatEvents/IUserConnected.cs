using Common.Interfaces;

namespace Chat.Interfaces.ChatEvents
{
	public interface IUserConnected : IEvent
    {
        IIdentifiable User { get; }
        string UserName { get; }
    }
}
