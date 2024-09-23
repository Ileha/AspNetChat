using AspNetChat.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class ChatRoomModel : PageModel
    {
        private readonly IChatContainer _chatContainer;

        public string MessagesList { get; private set; } = string.Empty;

        public ChatRoomModel(IChatContainer chatContainer)
        {
            _chatContainer = chatContainer ?? throw new ArgumentNullException(nameof(chatContainer));
        }

        public void OnGet()
        {
            Response.Redirect("/Chat");
        }

        public void OnGetJoinChatRoom(string chatName, string userName) 
        {
            var chatRoom = _chatContainer.GetChatByName(chatName);

            MessagesList = chatRoom.GetChatMessageList();
        }
    }
}
