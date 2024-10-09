namespace AspNetChat.Core.Interfaces.Services
{

	public interface IMessageListPublisherService
	{
		Task ConectWebSocket(string userID, string chatID, HttpContext context);
	}
}
