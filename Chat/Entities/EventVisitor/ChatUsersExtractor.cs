using System.Collections;
using Chat.Extensions.Comparers;
using Chat.Interfaces.ChatEvents;
using Common.Interfaces;

namespace Chat.Entities.EventVisitor;

public class ChatUsersExtractor : IEventVisitor, IEnumerable<IIdentifiable>
{
    private readonly HashSet<IIdentifiable> _usersSet = new(new IdentifiableEqualityComparer());
		
    public void Visit(IUserConnected userConnected)
    {
        _usersSet.Add(userConnected.User);
    }

    public void Visit(IUserSendMessage userSendMessage)
    {
    }

    public void Visit(IUserDisconnected userDisconnected)
    {
			
    }

    public IEnumerator<IIdentifiable> GetEnumerator()
    {
        return _usersSet.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}