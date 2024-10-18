using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Core.Interfaces
{
    public interface IChat : IIdentifiable
    {
		IReadOnlyList<IEvent> GetChatMessageList();
        bool HasPartisipant(IIdentifiable partisipant);

		void JoinParticipant(IChatPartisipant partisipant);
        void DisconnectedParticipant(IIdentifiable partisipant);
        void SendMessage(IIdentifiable partisipant, string message);
    }
}
