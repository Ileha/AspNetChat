using System.Net;
using Chat.Interfaces.Services;
using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace Chat.Services
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
