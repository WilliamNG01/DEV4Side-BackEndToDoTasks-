using AutoMapper.Configuration.Annotations;
using System.Text.Json.Serialization;

namespace WebAPITodoList.Models;

public partial class ToDoListRequest
{

    public string Name { get; set; } = null!;

    public int? UserId { get; set; }

}
public partial class ToDoListResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();

}


