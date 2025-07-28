using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;

namespace WebAPITodoList.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            // Using the repository to get all users
            var users = await _userRepository.GetAllAsync();
            var userdtos = _mapper.Map<IEnumerable<UserDto>>(users);
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return userdtos.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userRepository.GetByUserIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var userdto = _mapper.Map<UserDto>(user);
            return userdto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _userRepository.UpdateUserAsync(id, user);
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
           await _userRepository.CreateUserAsync(user);

            return CreatedAtAction("GetUserById", new { id = user.Id });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.DeleteUserAsync(id);
            if (!user)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
