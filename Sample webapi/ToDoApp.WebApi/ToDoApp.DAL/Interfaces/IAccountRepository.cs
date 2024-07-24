namespace ToDoApp.DAL.Interfaces;

/// <summary>
/// Interface defining operations related to account data access.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Registers a new user in the database.
    /// </summary>
    /// <param name="userDto">The UserDto object representing the user to register.</param>
    /// <returns>The ID of the newly registered user.</returns>
    Task<int> RegisterUser(UserDto userDto);

    /// <summary>
    /// Authenticates a user by verifying the provided username and password.
    /// </summary>
    /// <param name="username">The username of the user to authenticate.</param>
    /// <param name="password">The password of the user to authenticate.</param>
    /// <returns>True if authentication is successful, false otherwise.</returns>
    Task<bool> Authenticate(string username, string password);
}