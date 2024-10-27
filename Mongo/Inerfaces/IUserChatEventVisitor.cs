using Mongo.Entities;

namespace Mongo.Inerfaces
{
	public interface IUserChatEventVisitor
	{
		void Visit(UserJoined joined);
		void Visit(UserSendMessage sendMessage);
		void Visit(UserDisconnected disconnected);
	}
}
