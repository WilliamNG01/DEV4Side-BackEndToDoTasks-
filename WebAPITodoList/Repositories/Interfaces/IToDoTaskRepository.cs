using WebAPITodoList.DTOs;
using WebAPITodoList.Models;

namespace WebAPITodoList.Repositories.Interfaces;

public interface IToDoTaskRepository
{
    Task<IEnumerable<ToDoTaskDto>> GetTasksByListIdAsync(int listId, int userId);
    Task<ToDoTaskDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ToDoTaskRequest task);
    Task UpdateAsync(int taskId, ToDoTaskRequest task);
    Task DeleteAsync(ToDoTaskDto task);
    Task SaveChangesAsync();
}
