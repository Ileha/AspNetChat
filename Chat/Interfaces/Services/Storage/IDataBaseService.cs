using Common.Interfaces;

namespace Chat.Interfaces.Services.Storage
{
	public interface IDataBaseService
    {
        IChatStorage GetChatStorage(IIdentifiable chat);
    }
}
