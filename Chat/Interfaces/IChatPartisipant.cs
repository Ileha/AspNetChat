using Common.Interfaces;

namespace Chat.Interfaces
{
    public interface IChatPartisipant : IIdentifiable
    {
        string Name { get; }

		public record ParticipantParams(string name, Guid Guid);
	}
}
