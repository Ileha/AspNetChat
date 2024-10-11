using AspNetChat.Core.Factories;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.ChatEvents;
using AspNetChat.Core.Interfaces.Factories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class ChatRoomModel : PageModel
    {
        private readonly IChatContainer _chatContainer;
        private readonly IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant> _participantFactory;

        public IReadOnlyList<IEvent> MessagesList { get; private set; } = Array.Empty<IEvent>();
        public IChatPartisipant? ChatUser { get; private set; }
        public IChat? Chat { get; private set; }

        public ChatRoomModel(
            IChatContainer chatContainer,
            IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant> participantFactory
            )
        {
            _chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
            _participantFactory = participantFactory ?? throw new ArgumentNullException(nameof(participantFactory));
        }

        public void OnGet()
        {
            Response.Redirect("/Chat");
        }

        public void OnGetJoinChatRoom(string chatName, string userName)
        {
			Chat = _chatContainer.GetChatByName(chatName);

            ChatUser = _participantFactory.Create(new ParticipantFactory.ParticipantParams(userName));
			Chat.JoinParticipant(ChatUser);

            MessagesList = Chat.GetChatMessageList();
        }
    }
}
