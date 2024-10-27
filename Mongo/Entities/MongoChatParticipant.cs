using Chat.Interfaces;

namespace Mongo.Entities
{
    internal class MongoChatParticipant : IChatPartisipant
    {
        private readonly User _user;

        public string Name => _user.Name;
        public Guid Id => _user.Id;

        public MongoChatParticipant(User user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
