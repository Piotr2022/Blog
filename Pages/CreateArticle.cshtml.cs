using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace Blog.Pages
{
    public class Index1Model : PageModel
    {
       

      
        public IActionResult OnGet()
        {
            return Page();
        }

    
    }
}
