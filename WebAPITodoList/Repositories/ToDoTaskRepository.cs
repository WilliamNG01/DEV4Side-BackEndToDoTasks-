using Microsoft.EntityFrameworkCore;
using WebAPITodoList.Data;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Repositories;

public class ToDoTaskRepository : IToDoTaskRepository
{
    private readonly MyToDoDbContext _context;
    public ToDoTaskRepository(MyToDoDbContext context) => _context = context;

    public async Task<IEnumerable<ToDoTask>> GetTasksByListIdAsync(int listId) =>
        await _context.ToDoTasks.Where(t => t.ListId == listId).ToListAsync();

    public async Task<ToDoTask?> GetByIdAsync(int id) =>
        await _context.ToDoTasks.FindAsync(id);

    public async Task AddAsync(ToDoTask task) => await _context.ToDoTasks.AddAsync(task);

    public async Task UpdateAsync(ToDoTask task)
    {
        _context.ToDoTasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ToDoTask task)
    {
        _context.ToDoTasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
