using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Services.Storage;

namespace AspNetChat.Core.Interfaces
{
    public interface IChat : IIdentifiable
    {
		Task<IReadOnlyList<IEvent>> GetChatMessageList();
        bool HasPartisipant(IIdentifiable partisipant);

		Task<IChatPartisipant> JoinParticipant(IChatPartisipant partisipant);
		Task DisconnectedParticipant(IIdentifiable partisipant);
		Task SendMessage(IIdentifiable partisipant, string message);

		public record ChatParams(Guid Guid, IChatStorage ChatStorage);
	}
}
