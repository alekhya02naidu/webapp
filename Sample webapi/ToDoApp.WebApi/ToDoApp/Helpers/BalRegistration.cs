namespace ToDoApp.Helpers;
public static class BalRegistration
{
    public static IServiceCollection BalServiceRegistration(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITaskService, TaskService>();
        services.AddTransient<IAccountService, AccountService>();

        return services;
    }
}



