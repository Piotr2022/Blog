using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Blog.Pages
{
    public class CreateArticleModel : PageModel
    {
        [BindProperty]
        public Article Article { get; set; } = new Article();

        [BindProperty]
        public string TagNames { get; set; }

        private readonly ApplicationDbContext _context;

        public string Result { get; set; }

        public CreateArticleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Article.CreationDate = DateTime.Now;

                var tagNames = TagNames.Split(',').Select(t => t.Trim());

                _context.Articles.Add(Article);
                _context.SaveChanges(); 

                foreach (var tagName in tagNames)
                {
                    var existingTag = _context.Tags.FirstOrDefault(t => t.Name.ToUpper() == tagName.ToUpper());

                    if (existingTag == null)
                    {
                        existingTag = new Tag { Name = tagName };
                        _context.Tags.Add(existingTag);
                        _context.SaveChanges(); // Save changes to generate TagId
                    }

                }

                _context.SaveChanges();

                Result = "true";

                return RedirectToPage("/Article", new { id = Article.Id });
            }

            return Page();
        }
    }
}
