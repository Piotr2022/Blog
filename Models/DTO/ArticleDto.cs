using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DTO
{
    public class ArticleDto
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public String Body { get; set; }

        public String UserId { get; set; }

    }
}
