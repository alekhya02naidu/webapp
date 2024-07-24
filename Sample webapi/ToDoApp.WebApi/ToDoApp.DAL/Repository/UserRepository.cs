namespace ToDoApp.DAL.Repository;

/// <summary>
/// Repository class implementing IUserRepository for user data access operations.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ToDoAppDbContext _dbContext;
    private readonly UserMapper _userMapper;

    public UserRepository(ToDoAppDbContext dbContext, UserMapper userMapper)
    {
        _dbContext = dbContext;
        _userMapper = userMapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await _dbContext.Users.ToListAsync();
        return users.Select(u => _userMapper.MapUserToUserDto(u));
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserById(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if(user != null) 
        {
            return _userMapper.MapUserToUserDto(user);
        }
        return null;
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserFromName(string username)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if(user != null)
        {
            return _userMapper.MapUserToUserDto(user);
        }
        return null;
    }
}