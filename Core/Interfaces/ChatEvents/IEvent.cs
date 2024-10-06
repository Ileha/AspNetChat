namespace AspNetChat.Core.Interfaces.ChatEvents
{
    public interface IEvent
    {
        DateTime DateTime { get; }
        void Accept(IEventVisitor eventVisitor);
    }
}
