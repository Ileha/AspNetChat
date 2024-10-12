namespace AspNetChat.Core.Interfaces.Services
{

	public interface IMessageListPublisherService
	{
		Task ConnectWebSocket(string userID, string chatID, HttpContext context);
	}
}
