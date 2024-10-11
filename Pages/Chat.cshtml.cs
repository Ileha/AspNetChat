using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class ChatModel : PageModel
    {
        private const string GreetingMessage = "Enter chat room name and your name";
        private const string IncorrectFieldsError = "Incorrect Values Provided";

        public string Message = GreetingMessage;

        [BindProperty(SupportsGet = true)]
        public string ChatName { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPostChatting()
        {
            if (string.IsNullOrWhiteSpace(ChatName) || string.IsNullOrWhiteSpace(UserName))
            {
                Message = $"{GreetingMessage}\n{IncorrectFieldsError}";
                return Page();
            }

            return RedirectToPage("/ChatRoom", "JoinChatRoom", new { chatName = ChatName, userName = UserName });
		}

        public void OnPostChatting2()
        {
            Response.Redirect("/ChatRoom");
		}
	}
}
