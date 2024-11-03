using Chat.Entities;
using Chat.Interfaces.Services;
using Common.Entities;
using Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Chat.Services;

public class MessageListPublisherService : IMessageListPublisherService
{
	private readonly IUserConnectionService _userConnectionService;
	private readonly ChatUserHelper _chatUserHelper;

	public MessageListPublisherService(
		IUserConnectionService userConnectionService,
		ChatUserHelper chatUserHelper)
	{
		_userConnectionService = userConnectionService ?? throw new ArgumentNullException(nameof(userConnectionService));
		_chatUserHelper = chatUserHelper ?? throw new ArgumentNullException(nameof(chatUserHelper));
	}

	public async Task ConnectWebSocket(string userId, string chatId, HttpContext context)
	{
		if (!_chatUserHelper.GetUserAndChatId(userId, chatId, out var statusCode, out var message, out var userGuid, out var chatGuid)) 
		{
			context.Response.StatusCode = (int) statusCode;
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

		var wsContext = new WebSocketAcceptContext
		{
			KeepAliveInterval = TimeSpan.FromMinutes(1),
		};
		var webSocket = await context.WebSockets.AcceptWebSocketAsync(wsContext);
		
		var actualUserConnection = _userConnectionService.AddUserConnection(userAndChatResult.chat, (Identifiable)userGuid, webSocket);

		await HandleUserWebSocket(actualUserConnection, actualUserConnection.CancellationToken);
	}
	
	private async Task HandleUserWebSocket(UserConnection connection, CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				await connection.WebSocket.WaitMessageAsync(token);
			}
			catch (OperationCanceledException)
			{
				connection.Dispose();
				return;
			}
		}
	}
}