using INFORCE_.NET_TASK.Server.DbLogic;
using Microsoft.EntityFrameworkCore;

namespace INFORCE_.NET_TASK.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UrlShortenerContext _context;

        public UserRepository(UrlShortenerContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetByLoginAsync(string login)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<User> GetByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Users.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> CheckIfExistsUserWithSuchLogin(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }
    }
}
