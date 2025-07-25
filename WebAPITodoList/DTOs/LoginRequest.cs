using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class LoginRequest
{
    public required string UserNameOrEmail { get; set; }
    public required string Password { get; set; }
}
