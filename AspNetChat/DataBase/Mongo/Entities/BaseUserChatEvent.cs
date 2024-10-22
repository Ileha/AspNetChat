using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using AspNetChat.DataBase.Mongo.Inerfaces;

namespace AspNetChat.DataBase.Mongo.Entities
{
	public enum UserEventType
	{
		Joined = 0,
		Message = 1,
		Disconnected = 2,
	}

	public abstract class BaseUserChatEvent
	{
		[BsonId]
		[BsonGuidRepresentation(GuidRepresentation.Standard)]
		public Guid EventId { get; set; }
		public DateTime Time { get; set; }
		public UserEventType EventType { get; set; }

		[BsonGuidRepresentation(GuidRepresentation.Standard)]
		public Guid UserId { get; set; }

		[BsonGuidRepresentation(GuidRepresentation.Standard)]
		public Guid ChatId { get; set; }

		public abstract void Accept(IUserChatEventVisitor visitor);
	}

	public class UserJoined : BaseUserChatEvent
	{
		public override void Accept(IUserChatEventVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			visitor.Visit(this);
		}
	}

	public class UserSendMessage : BaseUserChatEvent
	{
		public string Message { get; set; }

		public override void Accept(IUserChatEventVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			visitor.Visit(this);
		}
	}

	public class UserDisconnected : BaseUserChatEvent
	{
		public override void Accept(IUserChatEventVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			visitor.Visit(this);
		}
	}
}
