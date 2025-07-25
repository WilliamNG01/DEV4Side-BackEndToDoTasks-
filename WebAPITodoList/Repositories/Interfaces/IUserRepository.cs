using WebAPITodoList.Models;

namespace WebAPITodoList.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUserIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);

        Task<bool> RegisterUserUserAsync(RegisterUserDto user);
        Task<int> LoginAsync(LoginRequest login);
    }
}
