using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public List<Article> ArticlesWithThisTag { get; set; }
    }
}
