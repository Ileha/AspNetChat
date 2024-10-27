using Chat.Entities.ChatModel;
using Chat.Entities.ChatModel.Events;
using Chat.Interfaces;
using Chat.Services;
using Common.Extensions.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Chat;

public class ChatInstaller : InstallerBase
{
    public ChatInstaller(IServiceCollection services) : base(services)
    {
        
    }
    
    public override void Install()
    {
        Services.AddSingleton<IChatContainer, ChatDataModel>();
        Services.AddFactoryTo<IChat.ChatParams, IChat, ChatModel>();
        Services.AddFactoryTo<IChatPartisipant.ParticipantParams, IChatPartisipant, ChatParticipant>();
        Services.BindSingletonInterfacesTo<MessageListPublisherService>();
        Services.AddSingleton<ChatUserHelper>();
        Services.BindSingletonInterfacesTo<DisconnectionService>();
        Services.BindSingletonInterfacesTo<MessageReceiverService>();
        Services.AddSingleton<ChatEventComposer>();
		
        Services.AddFactory<UserConnected.Params, UserConnected>();
        Services.AddFactory<UserConnected.NewParams, UserConnected>();
        Services.AddFactory<UserDisconnected.Params, UserDisconnected>();
        Services.AddFactory<UserDisconnected.NewParams, UserDisconnected>();
        Services.AddFactory<UserSendMessage.Params, UserSendMessage>();
        Services.AddFactory<UserSendMessage.NewParams, UserSendMessage>();
    }
}