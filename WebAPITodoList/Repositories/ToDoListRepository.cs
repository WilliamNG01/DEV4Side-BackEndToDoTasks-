using Microsoft.EntityFrameworkCore;
using WebAPITodoList.Data;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Repositories;

public class ToDoListRepository : IToDoListRepository
{
    private readonly MyToDoDbContext _context;
    public ToDoListRepository(MyToDoDbContext context) => _context = context;

    public async Task<IEnumerable<ToDoListResponse>> GetAllByUserIdAsync(int userId) =>
        await _context.ToDoLists
            .Include(x => x.ToDoTasks)
            .Where(x => x.UserId == userId)
            .Select(x => new ToDoListResponse
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                ToDoTasks = x.ToDoTasks
            })
            .ToListAsync();

    public async Task<ToDoList?> GetByIdAsync(int id) =>
        await _context.ToDoLists.FindAsync(id);

    public async Task AddAsync(ToDoList list) => await _context.ToDoLists.AddAsync(list);

    public async Task DeleteAsync(ToDoList list) => _context.ToDoLists.Remove(list);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
