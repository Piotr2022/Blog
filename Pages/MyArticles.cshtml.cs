using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.ContosoUniversity;

namespace Blog.Pages
{
    public class MyArticlesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaginatedList<Article> Articles { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public MyArticlesModel(ILogger<IndexModel> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        public async Task OnGetAsync(
            string sortOrder,
            string currentFilter, string searchString,
          int? pageIndex)
        {

            CurrentSort = sortOrder;
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<Article> my_articles = from a in _context.Articles where a.UserId == userID select a;
            my_articles = my_articles.OrderByDescending(a => a.CreationDate);

            var pageSize = _configuration.GetValue("PageSize", 10);
            Articles = await PaginatedList<Article>.CreateAsync(
                my_articles.AsNoTracking(), pageIndex ?? 1, pageSize);

        }
        public IActionResult OnPost(int id)
        {
            var entry = _context.Articles.Find(id);

            if (entry != null)
            {
                var articleTagsToRemove = _context.ArticleTag.Where(at => at.ArticleId == id).ToList();

                foreach (var articleTagToRemove in articleTagsToRemove)
                {
                    var isTagUsed = _context.ArticleTag.Any(at => at.TagId == articleTagToRemove.TagId && at.ArticleId != id);

                    if (!isTagUsed)
                    {
                        var tag = _context.Tags.Find(articleTagToRemove.TagId);
                        if (tag != null)
                        {
                            _context.Tags.Remove(tag);
                        }
                    }

                    _context.ArticleTag.Remove(articleTagToRemove);
                }

                _context.Articles.Remove(entry);
                _context.SaveChanges();
            }

            return RedirectToPage("/MyArticles");
        }


        //public IActionResult OnPostArticleId(int id)
        //{
        //    return RedirectToPage("/articleDto", new { id });
        //}
    }
}
