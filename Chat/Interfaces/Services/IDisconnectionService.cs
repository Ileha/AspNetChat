using Microsoft.AspNetCore.Http;

namespace Chat.Interfaces.Services
{

	public interface IDisconnectionService 
	{
		Task DisconnectUser(string userID, string chatID, HttpContext context);
	}
}
