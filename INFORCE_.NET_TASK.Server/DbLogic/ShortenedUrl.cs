using System.ComponentModel.DataAnnotations;

namespace INFORCE_.NET_TASK.Server.DbLogic
{
    public class ShortenedUrl
    {
        [Required]
        public string OriginalUrl {  get; set; }

        [Key]
        public string ShortUrl { get; set; }

        [Required]
        public DateOnly CreatedDate {  get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User? User { get; set; }

    }
}
