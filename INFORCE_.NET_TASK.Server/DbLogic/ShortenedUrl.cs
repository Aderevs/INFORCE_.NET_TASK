using System.ComponentModel.DataAnnotations;

namespace INFORCE_.NET_TASK.Server.DbLogic
{
    public class ShortenedUrl
    {
        public Guid Id { get; set; }

        [Required]
        public string OriginalUrl {  get; set; }

        [Required]
        [MaxLength(50)]
        public string ShortUrl { get; set; }

        [Required]
        public DateOnly CreatedDate {  get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User? User { get; set; }

    }
}
