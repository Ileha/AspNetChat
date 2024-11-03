using Chat.Entities.ChatModel;
using Chat.Entities.ChatModel.Events;
using Chat.Entities.EventVisitor;
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
        Services.Scan(scan =>
        {
            scan
                .FromAssemblyOf<ChatInstaller>()
                .AddClasses(classes => classes.AssignableTo<ChatDataModel>())
                .As<IChatContainer>()
                .WithScopedLifetime()
                
                .AddClasses(classes => classes.AssignableTo<ChatUserHelper>())
                .AsSelf()
                .WithScopedLifetime()
                
                .AddClasses(classes => classes.AssignableTo<MessageListPublisherService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                
                .AddClasses(classes => classes.AssignableTo<MessageConsumerService>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime()
                
                .AddClasses(classes => classes.AssignableTo<DisconnectionService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                
                .AddClasses(classes => classes.AssignableTo<MessageReceiverService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                
                ;
        });
        
        Services.AddSingleton<ChatEventComposer>();
        
        Services.AddFactoryTo<IChat.ChatParams, IChat, ChatModel>();
        Services.AddFactoryTo<IChatParticipant.ParticipantParams, IChatParticipant, ChatParticipant>();
        Services.AddFactory<ChatUsersExtractor>();
        Services.AddFactory<CurrentParticipantsCountExtractor>();
		
        Services.AddFactory<UserConnected.Params, UserConnected>();
        Services.AddFactory<UserConnected.NewParams, UserConnected>();
        Services.AddFactory<UserDisconnected.Params, UserDisconnected>();
        Services.AddFactory<UserDisconnected.NewParams, UserDisconnected>();
        Services.AddFactory<UserSendMessage.Params, UserSendMessage>();
        Services.AddFactory<UserSendMessage.NewParams, UserSendMessage>();
    }
}