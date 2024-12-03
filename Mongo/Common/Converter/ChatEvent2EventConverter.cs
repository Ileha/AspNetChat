using Chat.Entities.ChatModel.Events;
using Chat.Interfaces.ChatEvents;
using Common.Extensions.DI;
using Mongo.Entities;
using Mongo.Interfaces;
using UserDisconnected = Chat.Entities.ChatModel.Events.UserDisconnected;
using UserSendMessage = Chat.Entities.ChatModel.Events.UserSendMessage;

namespace Mongo.Common.Converter;

public class ChatEvent2EventConverter : IUserChatEventVisitor
{
    private readonly IFactory<UserConnected.Params, UserConnected> _userConnectedFactory;
    private readonly IFactory<UserSendMessage.Params,UserSendMessage> _userSendMessageFactory;
    private readonly IFactory<UserDisconnected.Params, UserDisconnected> _userDisconnected;
    private User? _eventUser;
    public IEvent? Event { get; private set; }

    public ChatEvent2EventConverter(
        IFactory<UserConnected.Params, UserConnected> userConnectedFactory,
        IFactory<UserSendMessage.Params, UserSendMessage> userSendMessageFactory,
        IFactory<UserDisconnected.Params, UserDisconnected> userDisconnected
    )
    {
        _userConnectedFactory = userConnectedFactory ?? throw new ArgumentNullException(nameof(userConnectedFactory));
        _userSendMessageFactory = userSendMessageFactory ?? throw new ArgumentNullException(nameof(userSendMessageFactory));
        _userDisconnected = userDisconnected ?? throw new ArgumentNullException(nameof(userDisconnected));
    }

    public void SetUser(User eventUser) 
    {
        _eventUser = eventUser ?? throw new ArgumentNullException(nameof(eventUser));
    }

    public void Visit(UserJoined joined)
    {
        CheckIDEquality(joined);

        Event = _userConnectedFactory.Create(
            new UserConnected.Params(
                joined.EventId, 
                joined.UserId, 
                _eventUser!.Name, 
                joined.Time));
    }

    public void Visit(Entities.UserSendMessage sendMessage)
    {
        CheckIDEquality(sendMessage);
				
        Event = _userSendMessageFactory.Create(
            new UserSendMessage.Params(
                sendMessage.EventId, 
                sendMessage.UserId, 
                sendMessage.Message,
                sendMessage.Time));
    }

    public void Visit(Entities.UserDisconnected disconnected)
    {
        CheckIDEquality(disconnected);

        Event = _userDisconnected.Create(new UserDisconnected.Params(disconnected.EventId, disconnected.UserId, disconnected.Time));
    }

    private void CheckIDEquality(BaseUserChatEvent @event) 
    {
        if (_eventUser == null || !@event.UserId.Equals(_eventUser.Id))
            throw new InvalidOperationException("event and user id's not equal");
    }
}