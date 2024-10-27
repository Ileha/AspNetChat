using Common.Interfaces;

namespace Chat.Interfaces.ChatEvents
{
	public interface IUserSendMessage : IEvent
    {
		IIdentifiable User { get; }
		string Message { get; }
    }
}
