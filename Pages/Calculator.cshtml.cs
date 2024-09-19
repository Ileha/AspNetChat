using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetChat.Pages
{
    public class CalculatorModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Enter values";
        }

        public void OnPost(float? num1, float? num2)
        {
            if (!num1.HasValue || !num2.HasValue) 
            {
                Message = "incorrect data";
                return;
            }

            var result = num1 + num2;

            Message = $"summ of {num1} and {num2} is {result}.";
        }
    }
}
