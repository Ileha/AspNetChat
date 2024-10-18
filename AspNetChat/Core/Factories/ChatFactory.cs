using AspNetChat.Core.Entities.Model;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using AspNetChat.Core.Interfaces.Services;
using static AspNetChat.Core.Factories.ChatFactory;

namespace AspNetChat.Core.Factories
{
    public class ChatFactory : IFactory<ChatParams, IChat>
    {
		private readonly IMessageConsumerService _messageConsumerService;

		public ChatFactory(IMessageConsumerService messageConsumerService) 
        {
			_messageConsumerService = messageConsumerService ?? throw new ArgumentNullException(nameof(messageConsumerService));
		}

        public IChat Create(ChatParams param1)
        {
            if (param1 == null)
                throw new ArgumentNullException(nameof(param1));

            return new ChatModel(param1.Guid, _messageConsumerService);
        }

        public record ChatParams(Guid Guid);
    }
}
