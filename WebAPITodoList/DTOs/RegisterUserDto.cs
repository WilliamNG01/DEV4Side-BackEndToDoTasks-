using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPITodoList.Models;

public partial class RegisterUserDto
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required DateTime BirthDate { get; set; }
    public required string Password { get; set; }

    [JsonIgnore]
    public string? Role { get; set; }
}
