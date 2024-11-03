using System.Net;
using Chat.Interfaces;
using Common.Entities;

namespace Chat.Services;

public class ChatUserHelper
{
    private readonly IChatContainer _chatContainer;

    public ChatUserHelper(IChatContainer chatContainer)
    {
        _chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
    }

    public bool GetUserAndChatId(
        string userId,
        string chatId,
        out HttpStatusCode statusCode,
        out string message,
        out Guid userGuid,
        out Guid chatGuid)
    {
        statusCode = HttpStatusCode.InternalServerError;
        message = string.Empty;
        userGuid = Guid.Empty;
        chatGuid = Guid.Empty;

        if (!Guid.TryParse(chatId, out chatGuid))
        {
            message = "incorrect chat id";

            statusCode = HttpStatusCode.BadRequest;

            return false;
        }

        if (!Guid.TryParse(userId, out userGuid))
        {
            message = "incorrect user id";

            statusCode = HttpStatusCode.BadRequest;

            return false;
        }

        statusCode = HttpStatusCode.OK;
        return true;
    }

    public async Task<(bool has, HttpStatusCode statusCode, string message, IChat? chat)> GetUserAndChat(
        Guid userGuid,
        Guid chatGuid
    )
    {
        var message = string.Empty;
        var statusCode = HttpStatusCode.InternalServerError;
        var chat = default(IChat);

        if (!await _chatContainer.HasChat(chatGuid))
        {
            statusCode = HttpStatusCode.NotFound;
            message = "chat not found";

            return (false, statusCode, message, chat);
        }

        chat = _chatContainer.GetChatById(chatGuid);

        if (!(await chat.HasPartisipant((Identifiable)userGuid)))
        {
            statusCode = HttpStatusCode.NotFound;
            message = "user not found";

            return (false, statusCode, message, chat);
        }

        return (false, statusCode, message, chat);
    }
}