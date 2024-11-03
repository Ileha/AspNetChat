using Common.Interfaces;

namespace Chat.Interfaces.Services.Storage
{
	public interface IUserStorage 
    {
        Task<IChatParticipant> AddOrGetParticipant(IIdentifiable identifiable, IChatParticipant participant);
    }
}
