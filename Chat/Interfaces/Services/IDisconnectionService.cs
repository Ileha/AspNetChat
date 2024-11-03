using Microsoft.AspNetCore.Http;

namespace Chat.Interfaces.Services
{

	public interface IDisconnectionService 
	{
		Task DisconnectUser(string userId, string chatId, HttpContext context);
	}
}
