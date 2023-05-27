using Microsoft.EntityFrameworkCore;

namespace Blog.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
        public String Title { get; set; }
        public String Body { get; set; }
        public List<byte[]> Images { get; set; }
    }
}
