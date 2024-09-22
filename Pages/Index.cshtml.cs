using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class IndexModel : PageModel
    {
		public void OnGet()
		{
			
		}
        public void OnPostCalculator()
        {
            Response.Redirect("/Calculator");
        }

        public void OnPostParameters()
        {
            Response.Redirect("/Parameters/5/Tom/30");
        }

        public void OnPostPerson()
        {
            Response.Redirect("/Person");
        }
    }
}
