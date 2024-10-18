namespace AspNetChat.Core.Interfaces.ChatEvents
{
	public interface IUserDisconnected : IEvent
    {
		IIdentifiable User { get; }
	}
}
