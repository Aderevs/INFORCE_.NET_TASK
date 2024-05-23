namespace INFORCE_.NET_TASK.Server
{
    public class ShortenedUrlDTO
    {
        Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortedUrl { get; set; }
        public DateOnly CreatedDate {  get; set; }
        public UserDTO User { get; set; }
    }
}
