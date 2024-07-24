using ToDoApp.DB.Models;

namespace ToDoApp.BAL.Interfaces;

/// <summary>
/// Interface defining operations related to user management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user details wrapped in a Task.</returns>
    Task<UserDto> GetUserById(int id);

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The user details wrapped in a Task.</returns>
    Task<UserDto> GetUserFromName(string username);
}