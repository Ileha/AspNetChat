namespace AspNetChat.Core.Interfaces.ChatEvents
{
	public interface IUserConnected : IEvent
    {
        IIdentifiable User { get; }
        string UserName { get; }
    }
}
