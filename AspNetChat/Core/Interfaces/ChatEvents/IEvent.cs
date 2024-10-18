namespace AspNetChat.Core.Interfaces.ChatEvents
{
    public interface IEvent : IIdentifiable
    {
        DateTime DateTime { get; }
        void Accept(IEventVisitor eventVisitor);
    }
}
