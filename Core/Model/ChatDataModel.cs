using AspNetChat.Core.Factories;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using System.Security.Cryptography;
using System.Text;

namespace AspNetChat.Core.Model
{

    public class ChatDataModel : IChatContainer
    {
        private readonly Dictionary<Guid, IChat> _chats = new();
        private readonly IFactory<ChatFactory.ChatParams, IChat> _chatFactory;

        public ChatDataModel(IFactory<ChatFactory.ChatParams, IChat> chatFactory) 
        {
            _chatFactory = chatFactory ?? throw new ArgumentNullException(nameof(chatFactory));
        }

        public IChat GetChatById(Guid chatId)
        {
            if (!_chats.TryGetValue(chatId, out var chat)) 
            {
                chat = _chatFactory.Create(new ChatFactory.ChatParams(chatId));
            }

            return chat;
        }

        public IChat GetChatByName(string name)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(name));
            var result = new Guid(hash);

            return GetChatById(result);
        }
    }
}
