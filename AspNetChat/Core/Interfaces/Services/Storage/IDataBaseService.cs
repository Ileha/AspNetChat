namespace AspNetChat.Core.Interfaces.Services.Storage
{
	public interface IDataBaseService
    {
        IChatStorage GetChatStorage(IIdentifiable chat);
    }
}
