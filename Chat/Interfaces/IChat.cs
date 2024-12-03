using Chat.Interfaces.ChatEvents;
using Chat.Interfaces.Services.Storage;
using Common.Interfaces;

namespace Chat.Interfaces
{
    public interface IChat : IIdentifiable
    {
		Task<IReadOnlyList<IEvent>> GetChatMessageList();
        Task<bool> HasParticipant(IIdentifiable participant);

		Task<IChatParticipant> JoinParticipant(IChatParticipant participant);
		Task DisconnectedParticipant(IIdentifiable participant);
		Task SendMessage(IIdentifiable participant, string message);

		public record ChatParams(Guid Guid, IChatStorage ChatStorage);
	}
}
