using System;
using System.Collections.Generic;

namespace WebAPITodoList.DTOs;

public partial class ToDoTaskDto: ToDoTaskRequest
{
    public int Id { get; set; }

}

public partial class ToDoTaskRequest
{

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = null!;

    public required int ListId { get; set; }

}
