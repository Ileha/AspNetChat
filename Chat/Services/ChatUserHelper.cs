using System.Net;
using Chat.Interfaces;
using Common.Entities;

namespace Chat.Services
{
    public class ChatUserHelper
    {
        private readonly IChatContainer _chatContainer;

        public ChatUserHelper(IChatContainer chatContainer)
        {
            _chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
        }

        public bool GetUserAndChatID(
            string userID,
            string chatID,
            out HttpStatusCode statusCode,
            out string message,
            out Guid userGuid,
            out Guid chatGuid)
        {
            statusCode = HttpStatusCode.InternalServerError;
            message = string.Empty;
            userGuid = Guid.Empty;
            chatGuid = Guid.Empty;

            if (!Guid.TryParse(chatID, out chatGuid))
            {
                message = "incorrect chat id";

                statusCode = HttpStatusCode.BadRequest;

                return false;
            }

            if (!Guid.TryParse(userID, out userGuid))
            {
                message = "incorrect user id";

                statusCode = HttpStatusCode.BadRequest;

                return false;
            }

            statusCode = HttpStatusCode.OK;
            return true;
        }

		public bool GetUserAndChat(
            Guid userGuid,
            Guid chatGuid,
            out HttpStatusCode statusCode,
            out string message,
            out IChat chat
            )
        {
            message = string.Empty;
            statusCode = HttpStatusCode.InternalServerError;
            chat = null!;

            if (!_chatContainer.HasChat(chatGuid))
            {
                statusCode = HttpStatusCode.NotFound;
                message = "chat not found";

                return false;
            }

            chat = _chatContainer.GetChatById(chatGuid);

            if (!chat.HasPartisipant((Identifiable)userGuid))
            {
                statusCode = HttpStatusCode.NotFound;
                message = "user not found";

                return false;
            }

            return true;
        }
    }
}
