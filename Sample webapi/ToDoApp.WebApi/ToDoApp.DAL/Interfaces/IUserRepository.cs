namespace ToDoApp.DAL.Interfaces;

/// <summary>
/// Interface defining operations related to user data access.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves all users from the database.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    Task<IEnumerable<UserDto>> GetAllUsers();

    /// <summary>
    /// Retrieves a user by their ID from the database.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user with the specified ID.</returns>
    Task<UserDto> GetUserById(int id);

    /// <summary>
    /// Retrieves a user by their username from the database.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>The user with the specified username.</returns>
    Task<UserDto> GetUserFromName(string username);
}