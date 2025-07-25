using System;
using System.Collections.Generic;

namespace WebAPITodoList.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int? CreatedById { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreateRoleDate { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
