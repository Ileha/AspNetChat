namespace AspNetChat.Core.Interfaces.Services
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
