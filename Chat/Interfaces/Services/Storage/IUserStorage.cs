using Common.Interfaces;

namespace Chat.Interfaces.Services.Storage
{
	public interface IUserStorage 
    {
        Task<IChatPartisipant> AddOrGetParticipant(IIdentifiable identifiable, IChatPartisipant partisipant);
    }
}
