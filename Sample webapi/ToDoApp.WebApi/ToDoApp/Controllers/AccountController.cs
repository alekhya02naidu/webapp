
namespace ToDoApp.Controllers;

[Route("v{version:ApiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserController> _logger;

    public AccountController(IUserService userService,IAccountService accountService,IConfiguration configuration, ILogger<UserController> logger)
    {
        _userService = userService;
        _accountService = accountService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Handles user registration.
    /// </summary>
    /// <param name="userDto">The user details for registration.</param>
    /// <returns>An ApiResponse containing the newly created user's ID or an error status.</returns>
    
    [AllowAnonymous]        
    [HttpPost("SignUp")]
    public async Task<ApiResponse<int>> RegisterUser([FromBody] UserDto userDto)
    {
        try
        {
            var userId = await _accountService.RegisterUser(userDto);
            if (userId > 0)
            {
                return GenerateResponse(userId);
            }
            else
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating the user");
            return GenerateResponse(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    /// <summary>
    /// Handles user login and JWT token generation.
    /// </summary>
    /// <param name="userDto">The user details for login.</param>
    /// <returns>An ApiResponse containing the JWT token or an error status.</returns>
    [AllowAnonymous]        
    [HttpPost("SignIn")]
    public async Task<ApiResponse<string>> Login(UserDto userDto)
    {
        var isAuthenticated = await _accountService.Authenticate(userDto.Username, userDto.PasswordHash);
        if (!isAuthenticated)
        {
            _logger.LogError("Invalid Username or password");
            return GenerateResponse<string>(null, ResponseStatus.Fail, ErrorCode.UnAuthorized);
        }
        var userDetail = await _userService.GetUserFromName(userDto.Username);
        if (userDetail == null)
        {
            return GenerateResponse<string>(null, ResponseStatus.Fail, ErrorCode.NotFound);
        }
        var tokenString = await GenerateToken(userDetail.Id, userDetail.Username);
        return GenerateResponse(tokenString);
    }

    /// <summary>
    /// Generates a JWT token for authenticated users.
    /// </summary>
    /// <param name="id">The user's ID.</param>
    /// <param name="Username">The user's username.</param>
    /// <returns>A JWT token as a string.</returns>
    private async Task<string> GenerateToken(int id, string Username)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, Username),
            new Claim("Id", id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: signingCredentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
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
}
