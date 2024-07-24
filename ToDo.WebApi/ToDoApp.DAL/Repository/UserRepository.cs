using Microsoft.EntityFrameworkCore;
using ToDoApp.DAL.Interfaces;
using ToDoApp.DAL.Mapper;
using ToDoApp.DB.Models;

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
        return await _dbContext.UserDtos.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserById(int id)
    {
        return await _dbContext.UserDtos.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<UserDto> GetUserFromName(string username)
    {
        return await _dbContext.UserDtos.FirstOrDefaultAsync(u => u.Username == username);
    }
}