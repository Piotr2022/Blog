using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Pages
{
    public class CreateArticle : PageModel
    {
        [BindProperty]
        public Article Article { get; set; } = new Article();

        private readonly ApplicationDbContext _context;

        public string? Result { get; set; }
        public CreateArticle(ApplicationDbContext context)
        {
            _context=context;
        }
        public void OnGet()
        {

        }


        public IActionResult OnPost()
        {
            
            if (ModelState.IsValid)
            {
              
                Article.CreationDate = DateTime.Now;
              
                _context.Articles.Add(Article);

                _context.SaveChanges();
                Result = "true";


                return RedirectToPage("/Article", new { Article.Id });
            }
            return Page();



        }
    }
}
