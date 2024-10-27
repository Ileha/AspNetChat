using Chat.Interfaces.ChatEvents;
using Common.Entities;
using Common.Interfaces;

namespace Chat.Entities.ChatModel.Events
{
    public class UserConnected : IUserConnected
    {
        public DateTime DateTime { get; }

        public IIdentifiable User { get; }

        public string UserName { get; }

        public Guid Id { get; }

		public UserConnected(NewParams newParams)
        {
            UserName = newParams.UserName ?? throw new ArgumentNullException(nameof(newParams.UserName));
            User = (Identifiable) newParams.UserId;
            DateTime = newParams.DateTime;
            Id = Guid.NewGuid();
        }

		public UserConnected(Params @params)
        {
            UserName = @params.UserName ?? throw new ArgumentNullException(nameof(@params.UserName));
            User = (Identifiable) @params.UserId;
            DateTime = @params.DateTime;
            Id = @params.EventId;
        }

        public void Accept(IEventVisitor eventVisitor)
        {
            if (eventVisitor == null)
                throw new ArgumentNullException(nameof(eventVisitor));

            eventVisitor.Visit(this);
        }
        
        public record NewParams(Guid UserId, string UserName, DateTime DateTime);
        public record Params(Guid EventId, Guid UserId, string UserName, DateTime DateTime);
    }
}
