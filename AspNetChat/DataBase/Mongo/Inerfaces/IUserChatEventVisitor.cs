using AspNetChat.DataBase.Mongo.Entities;

namespace AspNetChat.DataBase.Mongo.Inerfaces
{
	internal interface IUserChatEventVisitor
	{
		void Visit(UserJoined joined);
		void Visit(UserSendMessage sendMessage);
		void Visit(UserDisconnected disconnected);
	}
}
