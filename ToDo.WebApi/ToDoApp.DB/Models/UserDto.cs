using System;
using System.Collections.Generic;

namespace ToDoApp.DB.Models;

public partial class UserDto
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
