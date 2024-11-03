using Chat.Interfaces;
using static Chat.Interfaces.IChatParticipant;

namespace Chat.Entities.ChatModel
{
    public class ChatParticipant : IChatParticipant
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
