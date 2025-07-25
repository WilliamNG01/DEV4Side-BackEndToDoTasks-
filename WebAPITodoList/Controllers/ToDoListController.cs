using Microsoft.AspNetCore.Mvc;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Controllers;


    [ApiController]
    [Route("lists")]
    public class ToDoListsController : Controller
{
        private readonly IToDoListRepository _listRepo;

        public ToDoListsController(IToDoListRepository listRepo) => _listRepo = listRepo;

        [HttpGet]
        public async Task<IActionResult> GetLists()
        {
            int userId = GetUserId(); // Implementa metodo per recuperare userId
            var lists = await _listRepo.GetAllByUserIdAsync(userId);
            return Ok(lists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] ToDoList list)
        {
            list.UserId = GetUserId(); // associa utente
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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst("id")?.Value ?? "0");
        }
    }

