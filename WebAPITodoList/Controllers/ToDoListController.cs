using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Controllers;

[Authorize]
[ApiController]
[Route("lists")]
public class ToDoListsController : Controller
{
    private readonly IToDoListRepository _listRepo;

    public ToDoListsController(IToDoListRepository listRepo) => _listRepo = listRepo;

    [HttpGet]
    public async Task<IActionResult> GetLists()
    {
        int userId = GetCurentUserId(); // Implementa metodo per recuperare userId
        var lists = await _listRepo.GetAllByUserIdAsync(userId);
        return Ok(lists);
    }

    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] ToDoListRequest dto)
    { // Associa la lista all'utente corrente
        var list = new ToDoList
        {
            Name = dto.Name,
            UserId = GetCurentUserId()
        };
        await _listRepo.AddAsync(list);
        await _listRepo.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLists), new { id = list.Id }, list);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteList(int id)
    {
        var list = await _listRepo.GetByIdAsync(id);
        if (list == null) return NotFound();
        await _listRepo.DeleteAsync(list);
        await _listRepo.SaveChangesAsync();
        return NoContent();
    }

    private int GetCurentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Convert.ToInt32(userId);
    }
}

