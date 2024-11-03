using Microsoft.AspNetCore.Http;

namespace Chat.Interfaces.Services
{
	public interface IMessageReceiverService 
	{
		Task ReceiveMessage(
			string userId, 
			string chatId, 
			string message,
			HttpContext context);
	}
}
