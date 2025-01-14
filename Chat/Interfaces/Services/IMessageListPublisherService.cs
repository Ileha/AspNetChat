﻿using Microsoft.AspNetCore.Http;

namespace Chat.Interfaces.Services
{

	public interface IMessageListPublisherService
	{
		Task ConnectWebSocket(string userID, string chatID, HttpContext context);
	}
}
