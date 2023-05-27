namespace Blog.Models
{
    public class Comment
    { 
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CommentedBodyId { get; set; } // bo moze byc albo artykul albo inny komentarz
        public string Body { get; set; }
    }
}
