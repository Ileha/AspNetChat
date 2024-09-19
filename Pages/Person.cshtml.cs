using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
    }

    public class PersonModel : PageModel
    {
        public string Message { get; set; }

        [BindProperty]
        public Person Person { get; set; } = new Person();

        public void OnGet()
        {
            Message = "Enter Data";
        }
        public void OnPost()
        {
            Message = $"Name: {Person.Name}  Age: {Person.Age}";
        }
    }
}
