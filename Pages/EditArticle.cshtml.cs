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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Article article = _context.Articles.Find(articleDto.Id);

            if (article != null)
            {
                article.Title = articleDto.Title;
                article.Body = articleDto.Body;

                // Usuñ po³¹czenia tagów dla artyku³u przed edycj¹
                var existingArticleTags = _context.ArticleTag.Where(at => at.ArticleId == articleDto.Id);
                foreach (var existingArticleTag in existingArticleTags)
                {
                    _context.ArticleTag.Remove(existingArticleTag);
                    article.ArticleTagConnection.Remove(existingArticleTag);
                }


                // Dodaj nowe po³¹czenia tagów
                if (!string.IsNullOrEmpty(articleDto.ArticleTagsNames))
                {
                    var articleTagNames = articleDto.ArticleTagsNames.Split(',').Select(t => t.Trim());

                    foreach(var tagName in articleTagNames)
                    {
                        var existingTag = _context.Tags.FirstOrDefault(t => t.Name.ToUpper() == tagName.ToUpper());

                        if (existingTag == null)
                        {
                            existingTag = new Tag { Name = tagName };
                            _context.Tags.Add(existingTag);
                            _context.SaveChanges();
                        }
                        var articleTag = new ArticleTag { ArticleId = articleDto.Id, TagId = existingTag.Id};
                        _context.ArticleTag.Add(articleTag);
                        _context.SaveChanges();
                    }
                }


                // Usuñ nieu¿ywane tagi

                List<Tag> allTags = _context.Tags.ToList();
                bool isTagUsed;
                foreach (var existingArticleTag in allTags)
                {
                    isTagUsed = _context.ArticleTag.Any(at => at.TagId == existingArticleTag.Id);
                    if (!isTagUsed)
                    {
                        _context.Tags.Remove(existingArticleTag);
                    }

                }
                /*
                foreach (var tag in tagsToRemove)
                {
                    _context.Tags.Remove(tag);
                }

                _context.SaveChanges();
                *//*
                tagsToRemove = existingArticleTags.Select(at => at.TagId).Distinct();
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
                }*/

                _context.SaveChanges();


                return RedirectToPage("Article", new { articleDto.Id });
            }

            return NotFound();
        }
    }
}
