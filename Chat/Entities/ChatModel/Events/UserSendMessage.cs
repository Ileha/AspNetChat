using Chat.Interfaces.ChatEvents;
using Common.Entities;
using Common.Interfaces;

namespace Chat.Entities.ChatModel.Events
{
    public class UserSendMessage : IUserSendMessage
	{
        public DateTime DateTime { get; }

        public IIdentifiable User { get; }

        public string Message { get; }

        public Guid Id { get; }

        public UserSendMessage(NewParams newParams)
        {
            Message = newParams.Message ?? throw new ArgumentNullException(nameof(newParams.Message));
            User = (Identifiable) newParams.UserId;
            DateTime = newParams.DateTime;
            Id = Guid.NewGuid();
        }

        public UserSendMessage(Params @params)
        {
            Message = @params.Message ?? throw new ArgumentNullException(nameof(@params.Message));
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

        public record NewParams(Guid UserId, string Message, DateTime DateTime);
        public record Params(Guid EventId, Guid UserId, string Message, DateTime DateTime);
    }
}
