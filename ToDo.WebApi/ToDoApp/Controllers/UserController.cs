using Microsoft.AspNetCore.Mvc;
using ToDoApp.BAL.Interfaces;
using ToDoApp.DB.Models;
using ToDoApp.ResponseModel.Enums;
using ToDoApp.ResponseModel;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Generates a standardized API response.
    /// </summary>
    /// <typeparam name="T">The type of the response data.</typeparam>
    /// <param name="data">The response data.</param>
    /// <param name="status">The status of the response (default is Success).</param>
    /// <param name="errorCode">Optional error code in case of an error.</param>
    /// <returns>An ApiResponse object containing the specified data, status, and error code.</returns>
    private ApiResponse<T> GenerateResponse<T>(T data, ResponseStatus status = ResponseStatus.Success, ErrorCode? errorCode = null)
    {
        return new ApiResponse<T>
        (
            status,
            data,
            errorCode
        );
    }

    /// <summary>
    /// Retrieves a user by username from the business layer.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>An ApiResponse containing the user information or an error status.</returns>
    [HttpGet("getUser/{username}")]
    public async Task<ApiResponse<UserDto>> GetUserFromName(string username)
    {
        try
        {
            var users = await _userService.GetUserFromName(username);
            return GenerateResponse(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all users");
            return GenerateResponse<UserDto>(null, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    /// <summary>
    /// Retrieves a user by ID from the business layer.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>An ApiResponse containing the user information or an error status.</returns>
    [HttpGet("{id}")]
    public async Task<ApiResponse<UserDto>> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return GenerateResponse<UserDto>(null!, ResponseStatus.Error, ErrorCode.NotFound);
            }
            return GenerateResponse(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching the user by ID");
            return GenerateResponse<UserDto>(null!, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

}
