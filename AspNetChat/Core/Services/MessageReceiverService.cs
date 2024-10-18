using AspNetChat.Core.Entities;
using AspNetChat.Core.Interfaces.Services;
using AspNetChat.Extensions;
using System.Net;

namespace AspNetChat.Core.Services
{
	public class MessageReceiverService : IMessageReceiverService
	{
		private readonly ChatUserHelper _chatUserHelper;

		public MessageReceiverService(ChatUserHelper chatUserHelper) 
		{
			_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
		}

		public async Task ReceiveMessage(string userID, string chatID, string message, HttpContext context)
		{
			var errorMessage = string.Empty;
			var statusCode = HttpStatusCode.InternalServerError;

			if (!_chatUserHelper.GetUserAndChatID(userID, chatID, out statusCode, out errorMessage, out var userGuid, out var chatGuid))
			{
				context.Response.StatusCode = (int)statusCode;
				await context.Response.WriteAsync(errorMessage);

				return;
			}

			if (!_chatUserHelper.GetUserAndChat(userGuid, chatGuid, out statusCode, out errorMessage, out var chat))
			{
				context.Response.StatusCode = (int)statusCode;
				await context.Response.WriteAsync(errorMessage);

				return;
			}

			chat.SendMessage((Identifiable)userGuid, message);
		}
	}
}
