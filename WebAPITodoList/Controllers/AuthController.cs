using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebAPITodoList.Models;
using WebAPITodoList.Repositories.Interfaces;
using WebAPITodoList.Services;

namespace WebAPITodoList.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    public readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    public AuthController(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registeruser)
    {
            var registered = await _userRepository.RegisterUserUserAsync(registeruser);

            if (!registered)
            {
                return BadRequest("Registrazione fallita");
            }
            return Ok("Registrazione completata");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        if(string.IsNullOrEmpty(login.UserNameOrEmail)) return Unauthorized();
        if(string.IsNullOrEmpty(login.Password)) return Unauthorized();
        var loged = await _userRepository.LoginAsync(login);
        if (loged>0)
        {
            User? user = await _userRepository.GetByUserIdAsync(loged);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
