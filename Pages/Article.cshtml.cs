using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Blog.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ArticleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Article Article { get; set; }

        public IActionResult OnGet(int id)
        {
            Article = _context.Articles.Find(id);

            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
