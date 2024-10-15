using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Core.Entities.ChatModel.Events
{
    public class UserConnected : IUserConnected
    {
        private readonly string _name;
        private readonly Guid _guid;

        public DateTime DateTime { get; }

		public IIdentifiable User => (Identifiable) _guid;

		public string UserName => _name;

		public Guid Id { get; }

		public UserConnected(string name, Guid guid, DateTime dateTime)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _guid = guid;
            DateTime = dateTime;
            Id = Guid.NewGuid();
        }

        public void Accept(IEventVisitor eventVisitor)
        {
            if (eventVisitor == null)
                throw new ArgumentNullException(nameof(eventVisitor));

            eventVisitor.Visit(this);
        }
    }
}
