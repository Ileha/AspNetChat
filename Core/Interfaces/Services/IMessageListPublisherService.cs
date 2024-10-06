namespace AspNetChat.Core.Interfaces.Services
{

	public interface IMessageListPublisherService
	{
		Task InvokeAsync(string userID, string chatID, HttpContext context);
	}
}
