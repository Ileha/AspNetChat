using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Core.Interfaces.Services
{
	public interface IMessageConsumerService 
	{
		void ConsumeMessage(IChat chat, IReadOnlyList<IEvent> events);
	}
}
