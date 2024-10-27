using Chat.Interfaces.ChatEvents;

namespace Chat.Interfaces.Services
{
	public interface IMessageConsumerService 
	{
		void ConsumeMessage(IChat chat, IReadOnlyList<IEvent> events);
	}
}
