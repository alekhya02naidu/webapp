namespace ToDoApp.BAL.Interfaces;

/// <summary>
/// Interface defining operations related to user account management.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userDto">The user details to register.</param>
    /// <returns>The ID of the newly registered user.</returns>
    Task<int> RegisterUser(UserDto userDto);

    /// <summary> 
    /// Authenticates a user based on username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>True if authentication is successful; false otherwise.</returns>
    Task<bool> Authenticate(string username, string password);
}