using Microsoft.AspNetCore.Http;

namespace Chat.Interfaces.Services
{

	public interface IMessageListPublisherService
	{
		Task ConnectWebSocket(string userId, string chatId, HttpContext context);
	}
}
