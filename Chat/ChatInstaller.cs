using Autofac;
using Chat.Entities.ChatModel;
using Chat.Entities.ChatModel.Events;
using Chat.Entities.EventVisitor;
using Chat.Interfaces;
using Chat.Interfaces.Services;
using Chat.Services;
using Common.Extensions.DI;

namespace Chat;

public class ChatInstaller : AutofacInstallerBase
{
    public ChatInstaller(ContainerBuilder builder) 
        : base(builder)
    {
    }

    public override void Install()
    {
        Builder
            .RegisterType<ChatEventComposer>()
            .AsSelf()
            .SingleInstance();
        
        Builder
            .RegisterType<ChatDataModel>()
            .As<IChatContainer>()
            .InstancePerLifetimeScope();

        Builder
            .RegisterType<ChatUserHelper>()
            .AsSelf()
            .InstancePerLifetimeScope();
        
        Builder
            .RegisterType<MessageListPublisherService>()
            .As<IMessageListPublisherService>()
            .InstancePerLifetimeScope();
        
        Builder
            .RegisterType<MessageConsumerService>()
            .AsImplementedInterfaces()
            .SingleInstance();
        
        Builder
            .RegisterType<DisconnectionService>()
            .As<IDisconnectionService>()
            .InstancePerLifetimeScope();
        
        Builder
            .RegisterType<MessageReceiverService>()
            .As<IMessageReceiverService>()
            .InstancePerLifetimeScope();
        
        Builder
            .AddFactoryTo<IChat.ChatParams, IChat, ChatModel>()
            .InstancePerLifetimeScope();
        
        Builder
            .AddFactoryTo<IChatParticipant.ParticipantParams, IChatParticipant, ChatParticipant>()
            .SingleInstance();
        
        Builder
            .AddFactory<ChatUsersExtractor>()
            .SingleInstance();
        
        Builder
            .AddFactory<CurrentParticipantsCountExtractor>()
            .SingleInstance();
        
        //---
        
        Builder
            .AddFactory<UserConnected.Params, UserConnected>()
            .SingleInstance();
        
        Builder
            .AddFactory<UserConnected.NewParams, UserConnected>()
            .SingleInstance();
        
        Builder
            .AddFactory<UserDisconnected.Params, UserDisconnected>()
            .SingleInstance();
        
        Builder
            .AddFactory<UserDisconnected.NewParams, UserDisconnected>()
            .SingleInstance();
        
        Builder
            .AddFactory<UserSendMessage.Params, UserSendMessage>()
            .SingleInstance();
        
        Builder
            .AddFactory<UserSendMessage.NewParams, UserSendMessage>()
            .SingleInstance();
    }
}