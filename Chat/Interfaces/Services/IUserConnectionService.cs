using System.Net.WebSockets;
using Chat.Entities;
using Chat.Services;
using Common.Interfaces;

namespace Chat.Interfaces.Services;

public interface IUserConnectionService
{
    UserConnection AddUserConnection(IIdentifiable chat, IIdentifiable user, WebSocket webSocket);
}