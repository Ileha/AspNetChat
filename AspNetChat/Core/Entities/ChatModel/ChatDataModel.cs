using AspNetChat.Core.Factories;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace AspNetChat.Core.Entities.Model
{

    public class ChatDataModel : IChatContainer
    {
        private readonly ConcurrentDictionary<Guid, IChat> _chats = new();
        private readonly IFactory<ChatFactory.ChatParams, IChat> _chatFactory;

        public ChatDataModel(IFactory<ChatFactory.ChatParams, IChat> chatFactory)
        {
            _chatFactory = chatFactory ?? throw new ArgumentNullException(nameof(chatFactory));
        }

        public IChat GetChatById(Guid chatId)
        {
			_chats.AddOrUpdate(
				chatId,
				(chatId) => _chatFactory.Create(new ChatFactory.ChatParams(chatId)),
				(chatId, item) => item);

			return _chats[chatId];
        }

        public IChat GetChatByName(string name)
        {
            var chatGuid = GetGuidFromName(name);
            return GetChatById(chatGuid);
        }

        public bool HasChat(string name)
        {
            var chatGuid = GetGuidFromName(name);

            return HasChat(chatGuid);
        }

        public bool HasChat(Guid chatId)
        {
            return _chats.ContainsKey(chatId);
        }

        private Guid GetGuidFromName(string name)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(name));
            return new Guid(hash);
        }
    }
}
