using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CommentedBodyId { get; set; } // bo moze byc albo artykul albo inny komentarz
        [StringLength(256, ErrorMessage = "Maksymalna długość znaków wynosi 256.")]

        public string Body { get; set; }
    }
}
