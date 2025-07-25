using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? UserName { get; set; }

    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public string Salt { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ToDoListum> ToDoLista { get; set; } = new List<ToDoListum>();

    public virtual ICollection<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
