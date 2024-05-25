namespace INFORCE_.NET_TASK.Server.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public bool IsAdmin { get; set; }
        public List<ShortenedUrlDTO> ShortedUrls { get; set; }

    }
}
