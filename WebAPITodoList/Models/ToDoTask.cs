using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class ToDoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    public string Status { get; set; } = null!;

    public int ListId { get; set; }

    public virtual ToDoList List { get; set; } = null!;
}
