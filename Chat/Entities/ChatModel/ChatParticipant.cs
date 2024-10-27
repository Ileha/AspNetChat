using Chat.Interfaces;
using static Chat.Interfaces.IChatPartisipant;

namespace Chat.Entities.ChatModel
{
    public class ChatParticipant : IChatPartisipant
    {
        public string Name { get; }

        public Guid Id { get; }

        public ChatParticipant(ParticipantParams participantParams)
        {
            if (participantParams == null)
                throw new ArgumentNullException(nameof(participantParams));

            Name = participantParams.name ?? throw new ArgumentNullException(nameof(participantParams.name));
            Id = participantParams.Guid;
        }
    }
}
