using INFORCE_.NET_TASK.Server.DbLogic;

namespace INFORCE_.NET_TASK.Server.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<bool> CheckIfExistsUserWithSuchLogin(string login);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByLoginAsync(string login);
        Task UpdateAsync(User user);
    }
}