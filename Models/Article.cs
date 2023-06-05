using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<Comment> Comments { get; set; }
        public ICollection<ArticleTag>? ArticleTagConnection { get; set; }
        public String Title { get; set; }
        public String Body { get; set; }
        public List<Image> Images { get; set; }
        public DateTime CreationDate { get; set; }

        public Article()
        {
            Images = new List<Image>();
            Comments = new List<Comment>();
        }
    }
}
