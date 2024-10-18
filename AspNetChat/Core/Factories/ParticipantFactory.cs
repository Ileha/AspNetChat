using AspNetChat.Core.Entities.Model;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using static AspNetChat.Core.Factories.ParticipantFactory;

namespace AspNetChat.Core.Factories
{
    public class ParticipantFactory : IFactory<ParticipantParams, IChatPartisipant> 
    {
        public IChatPartisipant Create(ParticipantParams param1)
        {
            if (param1 == null)
                throw new ArgumentNullException(nameof(param1));

            return new ChatParticipant(param1.name, Guid.NewGuid());
        }

        public record ParticipantParams(string name);
    }
}
