using Blog.Data;
using Blog.Models;
using Blog.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;

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

            if (article.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            var tags = _context.ArticleTag.Where(at => at.ArticleId == id).Select(at => at.Tag.Name).ToList();

            articleDto = new ArticleDto();
            articleDto.Id = article.Id;
            articleDto.Title = article.Title;
            articleDto.Body = article.Body;
            articleDto.ArticleTagsNames = string.Join(",", tags);
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

                // Usuñ po³¹czenia tagów dla artyku³u przed edycj¹
                var existingArticleTags = _context.ArticleTag.Where(at => at.ArticleId == articleDto.Id);
                foreach (var existingArticleTag in existingArticleTags)
                {
                    _context.ArticleTag.Remove(existingArticleTag);
                }


                // Dodaj nowe po³¹czenia tagów
                if (!string.IsNullOrEmpty(articleDto.ArticleTagsNames))
                {
                    var articleTagNames = articleDto.ArticleTagsNames.Split(',').Select(t => t.Trim());

                    foreach (var articleTagName in articleTagNames)
                    {
                        var tag = _context.Tags.FirstOrDefault(t => t.Name == articleTagName);
                        if (tag == null)
                        {
                            tag = new Tag { Name = articleTagName };
                            _context.Tags.Add(tag);
                        }

                        article.ArticleTagConnection.Add(new ArticleTag { Tag = tag });
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    throw new DbUpdateException("Error DataBase", e);
                }

                // Usuñ nieu¿ywane tagi
                var tagsToRemove = existingArticleTags.Select(at => at.TagId).Distinct();
                foreach (var tagId in tagsToRemove)
                {
                    var isTagUsed = _context.ArticleTag.Any(at => at.TagId == tagId && at.ArticleId != article.Id);
                    if (!isTagUsed)
                    {
                        var tagToRemove = _context.Tags.Find(tagId);
                        if (tagToRemove != null)
                        {
                            _context.Tags.Remove(tagToRemove);
                        }
                    }
                }

                return RedirectToPage("Article", new { articleDto.Id });
            }

            return NotFound();
        }
    }
}
