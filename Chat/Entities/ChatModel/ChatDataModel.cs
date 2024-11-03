using System.Security.Cryptography;
using System.Text;
using Chat.Interfaces;
using Chat.Interfaces.Services.Storage;
using Common.Entities;
using Common.Extensions.DI;
using static Chat.Interfaces.IChat;

namespace Chat.Entities.ChatModel;

public class ChatDataModel : IChatContainer
{
    private readonly IFactory<ChatParams, IChat> _chatFactory;
    private readonly IDataBaseService _chatDataBase;

    public ChatDataModel(
        IFactory<ChatParams, IChat> chatFactory, 
        IDataBaseService chatDataBase)
    {
        _chatFactory = chatFactory ?? throw new ArgumentNullException(nameof(chatFactory));
        _chatDataBase = chatDataBase ?? throw new ArgumentNullException(nameof(chatDataBase));
    }

    public IChat GetChatById(Guid chatId)
    {
        var chatStorage = _chatDataBase.GetChatStorage((Identifiable) chatId);

        return _chatFactory.Create(new ChatParams(chatId, chatStorage));
    }

    public IChat GetChatByName(string name)
    {
        var chatGuid = GetGuidFromName(name);
        return GetChatById(chatGuid);
    }

    public async Task<bool> HasChat(string name)
    {
        var chatGuid = GetGuidFromName(name);

        return await HasChat(chatGuid);
    }

    public async Task<bool> HasChat(Guid chatId)
    {
        return await _chatDataBase.HasChat((Identifiable) chatId);
    }

    private Guid GetGuidFromName(string name)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(name));
        return new Guid(hash);
    }
}