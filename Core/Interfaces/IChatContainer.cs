namespace AspNetChat.Core.Interfaces
{

    public interface IChatContainer
    {
        public IChat GetChatById(Guid chatId);
        public IChat GetChatByName(string name);

        public bool HasChat(string name);
        public bool HasChat(Guid chatId);
    }
}
