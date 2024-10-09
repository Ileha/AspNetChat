using AspNetChat.Core.Entities;
using AspNetChat.Core.Interfaces.Services;
using AspNetChat.Extensions;
using System.Net;

namespace AspNetChat.Core.Services
{
	public class DisconnectionService : IDisconnectionService
	{
		private readonly ChatUserHelper _chatUserHelper;

		public DisconnectionService(ChatUserHelper chatUserHelper) 
		{
			_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
		}

		public async Task DisconnectUser(string userID, string chatID, HttpContext context)
		{
			var message = string.Empty;
			var statusCode = HttpStatusCode.InternalServerError;

			if (!_chatUserHelper.GetUserAndChatID(userID, chatID, out statusCode, out message, out var userGuid, out var chatGuid))
			{
				context.Response.StatusCode = (int)statusCode;
				await context.Response.WriteAsync(message);

				return;
			}

			if (!_chatUserHelper.GetUserAndChat(userGuid, chatGuid, out statusCode, out message, out var chat))
			{
				context.Response.StatusCode = (int)statusCode;
				await context.Response.WriteAsync(message);

				return;
			}

			chat.DisconnectedParticipant((Identifiable) userGuid);
		}
	}
}
