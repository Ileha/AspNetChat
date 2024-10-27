using Microsoft.AspNetCore.Http;

namespace Chat.Interfaces.Services
{
	public interface IMessageReceiverService 
	{
		Task ReceiveMessage(
			string userID, 
			string chatID, 
			string message,
			HttpContext context);
	}
}
