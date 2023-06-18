using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Commentv2
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; } // komentarz
        [StringLength(256, ErrorMessage = "Maksymalna długość znaków wynosi 256.")]
        public string ArticleId { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ComputerIp { get; set; }

    }
}
