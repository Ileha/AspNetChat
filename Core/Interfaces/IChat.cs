namespace AspNetChat.Core.Interfaces
{
    public interface IChat : IIdentifiable
    {
        string GetChatMessageList();
        void JoinParticipant(IChatPartisipant partisipant);
        void DisconnectedParticipant(IChatPartisipant partisipant);
        void SendMessage(IChatPartisipant partisipant, string message);
    }
}
