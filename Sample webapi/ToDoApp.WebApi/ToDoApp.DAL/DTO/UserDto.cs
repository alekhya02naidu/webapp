namespace ToDoApp.DAL.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}
