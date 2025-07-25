using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAPITodoList.Data;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyToDoDbContext _context;
        private readonly string _connectionString;
        public UserRepository(MyToDoDbContext context)
        {
            _context = context;
        }
        public async System.Threading.Tasks.Task CreateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.Include(u => u.UserRoles)
                                        .ThenInclude(ur => ur.Role).ToListAsync();
            return [.. users];
        }

        public async Task<User?> GetByUserIdAsync(int userId)
        {
            var user = await _context.Users
                                    .Include(u => u.UserRoles)
                                        .ThenInclude(ur => ur.Role)
                                    .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(int id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task<bool> RegisterUserUserAsync(RegisterUserDto user)
        {
            bool result;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_login", connection))
                {

                    command.CommandText = "sp_register_user";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                    command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                    command.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                    command.Parameters.Add(new SqlParameter("@BirthDate", user.BirthDate));
                    command.Parameters.Add(new SqlParameter("@Email", user.Email));
                    command.Parameters.Add(new SqlParameter("@Password", user.Password)); // non ancora hashato: sarà fatto nella SP
                    command.Parameters.Add(new SqlParameter("@RoleId", GetRoleId(null)));

                    await connection.OpenAsync();
                    var read = await command.ExecuteScalarAsync();
                    result = Convert.ToInt32(read) > 0;
                    await connection.CloseAsync();
                }
            }

            return result;
        }
        public async Task<int> LoginAsync(LoginRequest login)
        {
            var result = new List<UserDto>();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_login", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@LoginInput", login.UserNameOrEmail));
                    command.Parameters.Add(new SqlParameter("@Password", login.Password));

                    await connection.OpenAsync();
                    using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        result.Add(new UserDto
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            BirthDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            CreatedAt = reader.GetDateTime(5)
                        });
                    }
                    await connection.CloseAsync();
                }
            }            

            var user = result.FirstOrDefault();
            return user != null ? user.Id : 0;
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private int GetRoleId(string? rolename)
        {
            Role? role = _context.Roles.FirstOrDefault(e => e.RoleName == rolename);
            if (role == null)
            {
                return _context.Roles.First(e => e.RoleName.ToLower().Trim() == "default").Id;
            }
            return role.Id;
        }
    }
}
