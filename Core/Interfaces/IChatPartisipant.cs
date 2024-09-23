namespace AspNetChat.Core.Interfaces
{
    public interface IChatPartisipant : IIdentifiable
    {
        string Name { get; }
    }
}
