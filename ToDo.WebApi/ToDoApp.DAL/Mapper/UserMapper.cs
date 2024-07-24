using ToDoApp.DB.Models;

namespace ToDoApp.DAL.Mapper;

/// <summary>
/// Mapper class for mapping UserDto to User entity.
/// </summary>
public class UserMapper
{
    /// <summary>
    /// Maps a UserDto object to a User entity.
    /// </summary>
    /// <param name="userDto">The UserDto object to map.</param>
    /// <returns>The mapped User entity.</returns>
    public User MapUserDtoToUser(UserDto userDto)
    {
        var user = new User
        {
            Username = userDto.Username,
            PasswordHash = userDto.PasswordHash
        };
        return user;
    }

}