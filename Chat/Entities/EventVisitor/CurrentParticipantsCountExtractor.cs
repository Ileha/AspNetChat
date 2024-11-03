using Chat.Extensions.Comparers;
using Chat.Interfaces.ChatEvents;
using Common.Interfaces;

namespace Chat.Entities.EventVisitor;

public class CurrentParticipantsCountExtractor : IEventVisitor
{
    private readonly Dictionary<IIdentifiable, int> _participantsCount = new(new IdentifiableEqualityComparer());
    
    public IReadOnlyDictionary<IIdentifiable, int> ParticipantsCount => _participantsCount;
    
    public void Visit(IUserConnected userConnected)
    {
        if (!_participantsCount.TryGetValue(userConnected.User, out var count)) 
            count = 0;
        
        _participantsCount[userConnected.User] = ++count;
    }

    public void Visit(IUserSendMessage userSendMessage)
    {
    }

    public void Visit(IUserDisconnected userDisconnected)
    {
        if (!_participantsCount.TryGetValue(userDisconnected.User, out var count))
            return;
        
        _participantsCount[userDisconnected.User] = Math.Clamp(count - 1, 0, int.MaxValue);
    }
}