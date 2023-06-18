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

        [BindProperty]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public Article Article { get; set; }

        [BindProperty]
        public List<Comment> Comments { get; set; }

        [BindProperty]
        [MaxLength(256, ErrorMessage = "Maksymalna d≥ugoúÊ znakÛw wynosi 256.")]
        public string NewCommentBody { get; set; }

        public IActionResult OnGet(int id)
        {
            Article = _context.Articles.Find(id);

            if (Article == null)
            {
                return NotFound();
            }

            Comments = _context.Comments.Where(c => c.CommentedBodyId == id.ToString()).OrderByDescending(c => c.CreationDate).ToList();

            // Sprawdü czy komentarze istniejπ i przypisz do Model.Comments
            if (Comments == null)
            {
                Comments = new List<Comment>();
            }

            return Page();
        }

        [HttpPost]
        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(NewCommentBody))
            {
                // UtwÛrz nowy komentarz
                var comment = new Comment
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CommentedBodyId = Article.Id.ToString(),
                    Body = NewCommentBody,
                    CreationDate = DateTime.Now
                };

                // Dodaj komentarz do bazy danych
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }

            Article = _context.Articles.Find(Article.Id);
            Comments = _context.Comments.Where(c => c.CommentedBodyId == Article.Id.ToString()).OrderByDescending(c => c.CreationDate).ToList();
            NewCommentBody = string.Empty;

            return Page();
        }
    }
}
