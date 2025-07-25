using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class ToDoList
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();

    public virtual User User { get; set; } = null!;
}
