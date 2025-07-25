namespace WebAPITodoList.Models;

public partial class ToDoDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EndedAt { get; set; }

    public string Stato { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
