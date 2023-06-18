using Blog.Data;
using Blog.Models;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Blog.Pages
{
    public class EditArticleModel : PageModel
    {
        [BindProperty]
        public ArticleDto articleDto { get; set; }

        private readonly ApplicationDbContext _context;

        public EditArticleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            if(article.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            articleDto = new ArticleDto();
            articleDto.Id = article.Id;
            articleDto.Title = article.Title;
            articleDto.Body = article.Body;
            articleDto.UserId = article.UserId;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Article article = await _context.Articles.FindAsync(articleDto.Id);

            if (article != null)
            {
                article.Title = articleDto.Title;
                article.Body = articleDto.Body;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    throw new DbUpdateException("Error DataBase", e);
                }

                return RedirectToPage("Article", new { articleDto.Id });

            }

            return NotFound();
        }
    }
}