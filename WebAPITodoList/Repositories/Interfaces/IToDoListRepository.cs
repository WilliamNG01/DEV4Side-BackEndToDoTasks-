using WebAPITodoList.Models;

namespace WebAPITodoList.Repositories.Interfaces;

public interface IToDoListRepository
{
    Task<IEnumerable<ToDoListResponse>> GetAllByUserIdAsync(int userId);
    Task<ToDoList?> GetByIdAsync(int id);
    Task AddAsync(ToDoList list);
    Task DeleteAsync(ToDoList list);
    Task SaveChangesAsync();
}
