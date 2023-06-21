using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        [BindProperty]
        public Comment Comment { get; set; }

        public List<Comment> Comments { get; set; }

        public int idToPost { get; set; }

        public List<Tag> Tags { get; set; }

        public IActionResult OnGet(int id)
        {
            idToPost = id;
            Article = _context.Articles.Find(id);

            if (Article == null)
            {
                return NotFound();
            }

            Comments = _context.Comments.Where(c => c.CommentedBodyId == id.ToString()).OrderByDescending(c => c.CreationDate).ToList();

            if (Comments == null)
            {
                Comments = new List<Comment>();
            }

            Tags = _context.Tags.Where(t => _context.Set<ArticleTag>().Any(at => at.ArticleId == Article.Id && at.TagId == t.Id)).ToList();
            return Page();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Comment.CreationDate = DateTime.Now;
                _context.Comments.Add(Comment);
                _context.SaveChanges();
            }

            return RedirectToPage("/Article", new { idToPost });
        }
    }
}
