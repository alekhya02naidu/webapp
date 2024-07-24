namespace ToDoApp.Helpers;
public static class DalRegistration
{
    public static IServiceCollection DalServiceRegistration(this IServiceCollection services)
    {
        services.AddTransient<UserMapper>();
        services.AddTransient<TaskMapper>();

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ITaskRepository, TaskRepository>();
        services.AddTransient<IAccountRepository, AccountRepository>();

        return services;
    }
}

