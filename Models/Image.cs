namespace Blog.Models
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
