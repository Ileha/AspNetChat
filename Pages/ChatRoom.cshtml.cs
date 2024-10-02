using AspNetChat.Core.Factories;
using AspNetChat.Core.Interfaces;
using AspNetChat.Core.Interfaces.Factories;
using AspNetChat.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class ChatRoomModel : PageModel
    {
        private readonly IChatContainer _chatContainer;
		private readonly IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant> _participantFactory;

		public string MessagesList { get; private set; } = string.Empty;
        public IChatPartisipant? User { get; private set; }

        public ChatRoomModel(IChatContainer chatContainer, 
            IFactory<ParticipantFactory.ParticipantParams, IChatPartisipant> participantFactory)
        {
            _chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
			_participantFactory = participantFactory;
		}

        public void OnGet()
        {
            Response.Redirect("/Chat");
        }

        public void OnGetJoinChatRoom(string chatName, string userName) 
        {
            var chatRoom = _chatContainer.GetChatByName(chatName);

			User = _participantFactory.Create(new ParticipantFactory.ParticipantParams(userName));
            chatRoom.JoinParticipant(User);

			MessagesList = chatRoom.GetChatMessageList();
        }
    }
}
