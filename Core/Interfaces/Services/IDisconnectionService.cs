namespace AspNetChat.Core.Interfaces.Services
{
	public interface IDisconnectionService 
	{
		Task DisconnectUser(string userID, string chatID, HttpContext context);
	}
}
