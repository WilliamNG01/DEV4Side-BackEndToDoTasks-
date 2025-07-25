using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class ToDoListum
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EndedAt { get; set; }

    public string Stato { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
