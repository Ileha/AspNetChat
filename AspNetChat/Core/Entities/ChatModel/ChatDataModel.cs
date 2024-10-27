using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Services.Storage;
using AspNetChat.Extensions.DI;
using static AspNetChat.Core.Interfaces.IChat;

namespace AspNetChat.Core.Entities.ChatModel
{
	public class ChatDataModel : IChatContainer
    {
        private readonly ConcurrentDictionary<Guid, IChat> _chats = new();
        private readonly IFactory<ChatParams, IChat> _chatFactory;
		private readonly IDataBaseService _chatDataBase;

		public ChatDataModel(IFactory<ChatParams, IChat> chatFactory, IDataBaseService chatDataBase)
        {
            _chatFactory = chatFactory ?? throw new ArgumentNullException(nameof(chatFactory));
			_chatDataBase = chatDataBase ?? throw new ArgumentNullException(nameof(chatDataBase));
		}

        public IChat GetChatById(Guid chatId)
        {
            var chatStorage = _chatDataBase.GetChatStorage((Identifiable) chatId);

			_chats.AddOrUpdate(
				chatId,
				(chatId) => _chatFactory.Create(new ChatParams(chatId, chatStorage)),
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
