using Microsoft.AspNetCore.Mvc;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Controllers;

[ApiController]
    [Route("tasks")]
    public class TasksController : Controller
    {
        private readonly IToDoTaskRepository _taskRepo;

        public TasksController(IToDoTaskRepository taskRepo) => _taskRepo = taskRepo;

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] int listId)
        {
            var tasks = await _taskRepo.GetTasksByListIdAsync(listId);
            return Ok(tasks);
        }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] ToDoTask task)
    {
        await _taskRepo.AddAsync(task);
        await _taskRepo.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] ToDoTask updated)
    {
        var taska = await _taskRepo.GetByIdAsync(id);
        if (taska == null) return NotFound();

        taska.Description = updated.Description;
        taska.DueDate = updated.DueDate;
        taska.Status = updated.Status;

        await _taskRepo.UpdateAsync(taska);
        return NoContent();
    }

    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) return NotFound();
            await _taskRepo.DeleteAsync(task);
            return NoContent();
        }
    }