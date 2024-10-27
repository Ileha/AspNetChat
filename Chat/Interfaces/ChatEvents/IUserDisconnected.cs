using Common.Interfaces;

namespace Chat.Interfaces.ChatEvents
{
	public interface IUserDisconnected : IEvent
    {
		IIdentifiable User { get; }
	}
}
