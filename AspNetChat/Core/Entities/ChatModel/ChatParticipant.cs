using AspNetChat.Core.Interfaces;
using static AspNetChat.Core.Interfaces.IChatPartisipant;

namespace AspNetChat.Core.Entities.Model
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
