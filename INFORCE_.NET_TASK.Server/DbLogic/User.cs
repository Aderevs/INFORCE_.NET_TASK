using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace INFORCE_.NET_TASK.Server.DbLogic
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string? Login { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public Guid Salt { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;

        public List<ShortenedUrl> Urls { get; set; }
    }
}
