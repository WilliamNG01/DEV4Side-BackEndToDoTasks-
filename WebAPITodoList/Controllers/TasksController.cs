using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPITodoList.DTOs;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Controllers;

[ApiController]
[Route("tasks")]
[Authorize]
public class TasksController : Controller
{
    private readonly IToDoTaskRepository _taskRepo;

    public TasksController(IToDoTaskRepository taskRepo) => _taskRepo = taskRepo;

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] int listId)
    {
        var tasks = await _taskRepo.GetTasksByListIdAsync(listId, GetCurentUserId());
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] ToDoTaskRequest task)
    {
        var idTask = await _taskRepo.AddAsync(task);
        if (idTask == 0) return BadRequest("Errore durante la creazione del task");

        return CreatedAtAction(nameof(GetTasks), new { id = idTask }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] ToDoTaskRequest task)
    {
        if(id<1) return BadRequest("Id non valido");
        if (task == null) return BadRequest("Task non valido");

        var taska = await _taskRepo.GetByIdAsync(id);
        if (taska == null) return NotFound();

        await _taskRepo.UpdateAsync(id, task);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        if (id < 1) return BadRequest("Task non valido");

        var task = await _taskRepo.GetByIdAsync(id);
        if (task == null) return NotFound();
        await _taskRepo.DeleteAsync(task);
        return NoContent();
    }
    private int GetCurentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Convert.ToInt32(userId);
    }
}