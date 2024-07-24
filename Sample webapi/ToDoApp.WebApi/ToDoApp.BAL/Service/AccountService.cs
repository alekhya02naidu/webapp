namespace ToDoApp.BAL.Service;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    public AccountService(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;
    }

    /// <inheritdoc />
    public async Task<int> RegisterUser(UserDto userDto) 
    {
        return await _accountRepo.RegisterUser(userDto);
    }
    
    /// <inheritdoc />  
    public async Task<bool> Authenticate(string username, string password)
    {
        return await _accountRepo.Authenticate(username, password);
    }
    
}