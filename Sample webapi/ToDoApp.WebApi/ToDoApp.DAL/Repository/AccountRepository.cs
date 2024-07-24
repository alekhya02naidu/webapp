namespace ToDoApp.DAL.Repository;

/// <summary>
/// Repository class implementing IAccountRepository for account-related data access operations.
/// </summary>
public class AccountRepository : IAccountRepository
{
    private readonly ToDoAppDbContext _dbContext;
    private readonly UserMapper _userMapper;

    public AccountRepository(ToDoAppDbContext dbContext, UserMapper userMapper)
    {
        _dbContext = dbContext;
        _userMapper = userMapper;
    }

    /// <inheritdoc />
    public async Task<int> RegisterUser(UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto), "UserDto cannot be null");
        }

        var user = _userMapper.MapUserDtoToUser(userDto);
        user.PasswordHash = HashPassword(user.PasswordHash);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user.Id;
    }

    /// <inheritdoc />
    public async Task<bool> Authenticate(string username, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Username == username);
        if (user == null)
        {
            return false;
        }
        return VerifyPassword(password, user.PasswordHash);
    }

    /// <summary>
    /// Hashes a password using BCrypt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password against a hashed password using BCrypt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hashedPassword">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hashed password, false otherwise.</returns>
    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
