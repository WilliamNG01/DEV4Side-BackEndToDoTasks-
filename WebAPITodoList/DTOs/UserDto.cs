using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class UserDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<string> Roles { get; set; } = new();
}
