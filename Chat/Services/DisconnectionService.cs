using Chat.Interfaces.Services;
using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace Chat.Services;

public class DisconnectionService : IDisconnectionService
{
	private readonly ChatUserHelper _chatUserHelper;

	public DisconnectionService(ChatUserHelper chatUserHelper) 
	{
		_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
	}

	public async Task DisconnectUser(string userId, string chatId, HttpContext context)
	{
		if (!_chatUserHelper.GetUserAndChatId(userId, chatId, out var statusCode, out var message, out var userGuid, out var chatGuid))
		{
			context.Response.StatusCode = (int)statusCode;
			await context.Response.WriteAsync(message);

			return;
		}

		var userAndChatResult = await _chatUserHelper.GetUserAndChat(userGuid, chatGuid);

		if (!userAndChatResult.has || userAndChatResult.chat == null)
		{
			context.Response.StatusCode = (int)userAndChatResult.statusCode;
			await context.Response.WriteAsync(userAndChatResult.message);

			return;
		}

		await userAndChatResult.chat.DisconnectedParticipant((Identifiable) userGuid);
	}
}