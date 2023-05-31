namespace Blog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public List<Article> ArticlesWithThisTag { get; set; }
    }
}
