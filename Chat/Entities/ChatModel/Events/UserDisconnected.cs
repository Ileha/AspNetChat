using Chat.Interfaces.ChatEvents;
using Common.Entities;
using Common.Interfaces;

namespace Chat.Entities.ChatModel.Events
{
    public class UserDisconnected : IUserDisconnected
	{
        public DateTime DateTime { get; }

        public IIdentifiable User { get; }

        public Guid Id { get; }

		public UserDisconnected(NewParams newParams)
        {
            User = (Identifiable) newParams.UserId;
            DateTime = newParams.DateTime;
			Id = Guid.NewGuid();
		}

        public UserDisconnected(Params @params)
        {
            User = (Identifiable) @params.UserId;
            DateTime = @params.DateTime;
            Id = @params.EventId;
        }

        void IEvent.Accept(IEventVisitor eventVisitor)
        {
            if (eventVisitor == null)
                throw new ArgumentNullException(nameof(eventVisitor));

            eventVisitor.Visit(this);
        }
        
        public record NewParams(Guid UserId, DateTime DateTime);
        public record Params(Guid EventId, Guid UserId, DateTime DateTime);
    }
}
