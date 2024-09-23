using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using AspNetChat.Core.Model;
using static AspNetChat.Core.Factories.ChatFactory;

namespace AspNetChat.Core.Factories
{
    public class ChatFactory : IFactory<ChatParams, IChat>
    {
        public IChat Create(ChatParams param1)
        {
            if (param1 == null)
                throw new ArgumentNullException(nameof(param1));

            return new ChatModel(param1.Guid);
        }

        public record ChatParams(Guid Guid);
    }
}
