namespace Chat.Interfaces.ChatEvents
{
	public interface IEventVisitor
    {
        void Visit(IUserConnected userConnected);
        void Visit(IUserSendMessage userSendMessage);
        void Visit(IUserDisconnected userDisconnected);
    }
}
