using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services.Storage;
using Common.Interfaces;

namespace Chat.Interfaces
{
    public interface IChat : IIdentifiable
    {
		Task<IReadOnlyList<IEvent>> GetChatMessageList();
        Task<bool> HasPartisipant(IIdentifiable partisipant);

		Task<IChatParticipant> JoinParticipant(IChatParticipant participant);
		Task DisconnectedParticipant(IIdentifiable partisipant);
		Task SendMessage(IIdentifiable partisipant, string message);

		public record ChatParams(Guid Guid, IChatStorage ChatStorage);
	}
}
