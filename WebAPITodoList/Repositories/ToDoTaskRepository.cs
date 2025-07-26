using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using WebAPITodoList.Data;
using WebAPITodoList.DTOs;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Repositories;

public class ToDoTaskRepository : IToDoTaskRepository
{
    private readonly MyToDoDbContext _context;
    private readonly IMapper _mapper;
    public ToDoTaskRepository(MyToDoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ToDoTaskDto>> GetTasksByListIdAsync(int listId)
    {
        var tasks = await _context.ToDoTasks
            .Where(t => t.ListId == listId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);
    }

    public async Task<ToDoTaskDto?> GetByIdAsync(int id)
    {
        var entity = await _context.ToDoTasks.FindAsync(id);
        return entity == null ? null : _mapper.Map<ToDoTaskDto>(entity);
    }

    public async Task<int> AddAsync(ToDoTaskRequest task)
    {
        var dto = _mapper.Map<ToDoTaskDto>(task);
        var entity = _mapper.Map<ToDoTask>(dto);
        
        var result = await _context.ToDoTasks.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result == null? 0 : result.Entity.Id;
    }

    public async Task UpdateAsync(int taskId, ToDoTaskRequest task)
    {
        var dto = _mapper.Map<ToDoTaskDto>(task);
        dto.Id = taskId;
        var entity = await _context.ToDoTasks.FirstOrDefaultAsync(x => x.ListId == dto.ListId && x.Id == dto.Id);
        if (entity != null)
        {
            entity.Title = task.Title;
            entity.Description = task.Description;
            entity.DueDate = task.DueDate;
            entity.Status = task.Status;
            _context.ToDoTasks.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(ToDoTaskDto task)
    {
        var entity = await _context.ToDoTasks.FirstOrDefaultAsync(x => x.ListId == task.ListId && x.Id == task.Id);
        if (entity != null)
        {
            _context.ToDoTasks.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    public async Task<IEnumerable<ToDoTaskDto>> GetTasksByListIdAsync(int listId, int userId)
    {
        return await _context.ToDoTasks
            .Include(t => t.List)
            .Where(t => t.ListId == listId && t.List.UserId == userId)
            .Select(t => _mapper.Map<ToDoTaskDto>(t))
            .ToListAsync();
    }
}
