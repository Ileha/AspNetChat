using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Core.Entities.ChatModel.Events
{
    public class UserSendMessage : IUserSendMessage
	{
        private readonly Guid _guid;
        private readonly string _message;

        public DateTime DateTime { get; }

		public IIdentifiable User => (Identifiable) _guid;

		public string Message => _message;

		public Guid Id { get; }

		public UserSendMessage(Guid guid, string message, DateTime dateTime)
        {
            _guid = guid;
            _message = message ?? throw new ArgumentNullException(nameof(message));
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
