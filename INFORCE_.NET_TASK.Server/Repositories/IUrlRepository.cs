using INFORCE_.NET_TASK.Server.DbLogic;

namespace INFORCE_.NET_TASK.Server.Repositories
{
    public interface IUrlRepository
    {
        Task AddAsync(ShortenedUrl url);
        Task<bool> CheckIfExistsSuchOriginalUrl(string originalUrl);
        Task<bool> CheckIfExistsSuchShortUrl(string shortUrl);
        Task DeleteAsync(ShortenedUrl url);
        Task DeleteByIdAsync(Guid id);
        Task<IEnumerable<ShortenedUrl>> GetAllAsync();
        Task<IEnumerable<ShortenedUrl>> GetAllIncludesUsersAsync();
        Task<ShortenedUrl> GetByIdAsync(Guid id);
        Task<ShortenedUrl> GetByIdIncludesUserAsync(Guid id);
        Task<ShortenedUrl> GetByShortUrl(string shortUrl);
        Task UpdateAsync(ShortenedUrl url);
    }
}