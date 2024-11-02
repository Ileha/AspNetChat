using Chat.Interfaces.ChatEvents;
using Common.Interfaces;
using Mongo.Entities;

namespace Mongo.Common.Converter;

public class Event2ChatEventConverter : IEventVisitor
{
    private readonly IIdentifiable _chat;

    public BaseUserChatEvent? ChatEvent { get; private set; }

    public Event2ChatEventConverter(IIdentifiable chat) 
    {
        _chat = chat ?? throw new ArgumentNullException(nameof(chat));
    }

    public void Visit(IUserConnected userConnected)
    {
        ChatEvent = new UserJoined()
        {
            ChatId = _chat.Id,
            UserId = userConnected.User.Id,
            EventType = UserEventType.Joined,
            Time = userConnected.DateTime,
            EventId = userConnected.Id,
        };
    }

    public void Visit(IUserSendMessage userSendMessage)
    {
        ChatEvent = new UserSendMessage()
        {
            ChatId = _chat.Id,
            UserId = userSendMessage.User.Id,
            EventType = UserEventType.Message,
            Time = userSendMessage.DateTime,
            EventId = userSendMessage.Id,
            Message = userSendMessage.Message,
        };
    }

    public void Visit(IUserDisconnected userDisconnected)
    {
        ChatEvent = new UserDisconnected()
        {
            ChatId = _chat.Id,
            UserId = userDisconnected.User.Id,
            EventType = UserEventType.Disconnected,
            Time = userDisconnected.DateTime,
            EventId = userDisconnected.Id,
        };
    }
}