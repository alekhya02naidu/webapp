namespace ToDoApp.BAL.Service;

/// <summary>
/// Service class implementing IUserService for user management operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserById(int id)
    {
        return await _userRepo.GetUserById(id);
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserFromName(string username) 
    {
        return await _userRepo.GetUserFromName(username);
    }
}