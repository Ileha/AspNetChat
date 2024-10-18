using AspNetChat.Core.Interfaces;

namespace AspNetChat.Core.Entities.Model
{
    public class ChatParticipant : IChatPartisipant
    {
        public string Name { get; }

        public Guid Id { get; }

        public ChatParticipant(string name, Guid guid)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = guid;
        }
    }
}
