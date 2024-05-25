using INFORCE_.NET_TASK.Server.DbLogic;
using INFORCE_.NET_TASK.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Server.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly UrlShortenerContext _context;

        public UrlRepository(UrlShortenerContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ShortenedUrl>> GetAllAsync()
        {
            return await _context.Urls.ToListAsync();
        }
        public async Task<IEnumerable<ShortenedUrl>> GetAllIncludesUsersAsync()
        {
            return await _context.Urls
                .Include(url => url.User)
                .ToListAsync();
        }
        public async Task AddAsync(ShortenedUrl url)
        {
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
        }
        public async Task<ShortenedUrl> GetByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Urls.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<ShortenedUrl> GetByIdIncludesUserAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Urls
                .Include(url => url.User)
                .FirstOrDefaultAsync(url => url.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<ShortenedUrl> GetByShortUrl(string shortUrl)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Urls.FirstOrDefaultAsync(url => url.ShortUrl == shortUrl);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task UpdateAsync(ShortenedUrl url)
        {
            _context.Urls.Update(url);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(Guid id)
        {
            var url = await _context.Urls.FindAsync(id);
            if (url != null)
            {
                _context.Urls.Remove(url);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(ShortenedUrl url)
        {
            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckIfExistsSuchOriginalUrl(string originalUrl)
        {
            return await _context.Urls.AnyAsync(url => url.OriginalUrl == originalUrl);
        }
        public async Task<bool> CheckIfExistsSuchShortUrl(string shortUrl)
        {
            return await _context.Urls.AnyAsync(url => url.ShortUrl == shortUrl);
        }
    }
}
