using Chat.Interfaces.Services;
using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace Chat.Services;

public class MessageReceiverService : IMessageReceiverService
{
	private readonly ChatUserHelper _chatUserHelper;

	public MessageReceiverService(ChatUserHelper chatUserHelper) 
	{
		_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
	}

	public async Task ReceiveMessage(string userId, string chatId, string message, HttpContext context)
	{
		if (!_chatUserHelper.GetUserAndChatId(userId, chatId, out var statusCode, out var errorMessage, out var userGuid, out var chatGuid))
		{
			context.Response.StatusCode = (int)statusCode;
			await context.Response.WriteAsync(errorMessage);

			return;
		}
		
		var userAndChatResult = await _chatUserHelper.GetUserAndChat(userGuid, chatGuid);

		if (!userAndChatResult.has || userAndChatResult.chat == null)
		{
			context.Response.StatusCode = (int)userAndChatResult.statusCode;
			await context.Response.WriteAsync(userAndChatResult.message);

			return;
		}

		userAndChatResult.chat.SendMessage((Identifiable)userGuid, message);
	}
}