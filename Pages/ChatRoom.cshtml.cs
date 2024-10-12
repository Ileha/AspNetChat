using AspNetChat.Core.Factories;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using AspNetChat.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AspNetChat.Core.Interfaces.Services.ChatEventComposer;

namespace AspNetChat.Pages
{
    public class ChatRoomModel : PageModel
    {
        private readonly IChatContainer _chatContainer;
        private readonly IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant> _participantFactory;
		private readonly ChatEventComposer _chatEventComposer;

        public IReadOnlyList<BaseUserEvent> MessagesList 
            => Chat != null 
            ? _chatEventComposer.GetEvents(Chat.GetChatMessageList()).ToArray()
            : Array.Empty<BaseUserEvent>();
        public IChatPartisipant? ChatUser { get; private set; }
        public IChat? Chat { get; private set; }

		public ChatRoomModel(
            IChatContainer chatContainer,
            IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant> participantFactory,
			ChatEventComposer chatEventComposer
			)
        {
            _chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
            _participantFactory = participantFactory ?? throw new ArgumentNullException(nameof(participantFactory));
			_chatEventComposer = chatEventComposer ?? throw new ArgumentNullException(nameof(chatEventComposer));
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
        }
    }
}
