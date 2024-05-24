namespace INFORCE_.NET_TASK.Server
{
    public class ShortenedUrlDTO
    {
        public Guid? Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateOnly CreatedDate {  get; set; }
        public UserDTO User { get; set; }
    }
}
