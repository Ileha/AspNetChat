using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using AspNetChat.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static AspNetChat.Core.Services.ChatEventComposer;

namespace AspNetChat.Pages
{
	public class ChatRoomModel : PageModel
    {
        private readonly IChatContainer _chatContainer;
        private readonly IFactory<IChatPartisipant.ParticipantParams, IChatPartisipant> _participantFactory;
		private readonly ChatEventComposer _chatEventComposer;

        public IChatPartisipant? ChatUser { get; private set; }
        public IChat? Chat { get; private set; }

		public ChatRoomModel(
            IChatContainer chatContainer,
            IFactory<IChatPartisipant.ParticipantParams, IChatPartisipant> participantFactory,
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

            ChatUser = _participantFactory.Create(new IChatPartisipant.ParticipantParams(userName));
			Chat.JoinParticipant(ChatUser);
        }

        public async Task<IReadOnlyList<BaseUserEvent>> GetMessageList() 
        {
            return Chat != null
			    ? _chatEventComposer.GetEvents(await Chat.GetChatMessageList()).ToArray()
			    : Array.Empty<BaseUserEvent>();
		}
    }
}
