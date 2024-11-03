using Chat.Interfaces;
using Chat.Services;
using Common.Extensions.DI;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chat.Pages
{
	public class ChatRoomModel : PageModel
    {
        private readonly IChatContainer _chatContainer;
        private readonly IFactory<IChatParticipant.ParticipantParams, IChatParticipant> _participantFactory;
		private readonly ChatEventComposer _chatEventComposer;

        public IChatParticipant? ChatUser { get; private set; }
        public IChat? Chat { get; private set; }

		public ChatRoomModel(
            IChatContainer chatContainer,
            IFactory<IChatParticipant.ParticipantParams, IChatParticipant> participantFactory,
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

        public async Task OnGetJoinChatRoom(string chatName, string userName)
        {
			Chat = _chatContainer.GetChatByName(chatName);

            var tempUser = _participantFactory.Create(new IChatParticipant.ParticipantParams(userName, Guid.NewGuid()));
			ChatUser = await Chat.JoinParticipant(tempUser);
        }

        public async Task<IReadOnlyList<ChatEventComposer.BaseUserEvent>> GetMessageList() 
        {
            return Chat != null
			    ? _chatEventComposer.GetEvents(await Chat.GetChatMessageList()).ToArray()
			    : Array.Empty<ChatEventComposer.BaseUserEvent>();
		}
    }
}
