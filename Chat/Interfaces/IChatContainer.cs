namespace Chat.Interfaces;

public interface IChatContainer
{
    public IChat GetChatById(Guid chatId);
    public IChat GetChatByName(string name);

    public Task<bool> HasChat(string name);
    public Task<bool> HasChat(Guid chatId);
}