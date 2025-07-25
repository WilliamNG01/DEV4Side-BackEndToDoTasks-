using WebAPITodoList.Models;

namespace WebAPITodoList.Repositories.Interfaces;

public interface IToDoTaskRepository
{
    Task<IEnumerable<ToDoTask>> GetTasksByListIdAsync(int listId);
    Task<ToDoTask?> GetByIdAsync(int id);
    Task AddAsync(ToDoTask task);
    Task UpdateAsync(ToDoTask task);
    Task DeleteAsync(ToDoTask task);
    Task SaveChangesAsync();
}
