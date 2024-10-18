using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Core.Entities.ChatModel.Events
{
    public class UserDisconnected : IUserDisconnected
	{
        private readonly Guid _guid;

        public DateTime DateTime { get; }

		public IIdentifiable User => (Identifiable) _guid;

		public Guid Id { get; }

		public UserDisconnected(Guid guid, DateTime dateTime)
        {
            _guid = guid;
            DateTime = dateTime;
			Id = Guid.NewGuid();
		}

        void IEvent.Accept(IEventVisitor eventVisitor)
        {
            if (eventVisitor == null)
                throw new ArgumentNullException(nameof(eventVisitor));

            eventVisitor.Visit(this);
        }
    }
}
