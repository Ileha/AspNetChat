namespace AspNetChat.Core.Interfaces.ChatEvents
{
	public interface IUserSendMessage : IEvent
    {
		IIdentifiable User { get; }
		string Message { get; }
    }
}
