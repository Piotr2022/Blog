using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blog.Pages.Shared
{
    public class ViewArticlesModel : PageModel
    {
        public void OnGet()
        {
        }

        /*
         usuwanie
         [Authorize(Roles = "Administrator")]
         
         */
    }
}
