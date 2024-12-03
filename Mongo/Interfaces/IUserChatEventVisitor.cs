using Mongo.Entities;

namespace Mongo.Interfaces
{
	public interface IUserChatEventVisitor
	{
		void Visit(UserJoined joined);
		void Visit(UserSendMessage sendMessage);
		void Visit(UserDisconnected disconnected);
	}
}
