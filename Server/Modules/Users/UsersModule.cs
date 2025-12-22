namespace Server.Modules.Users;

/// <summary>
/// Module for registering user-related services.
/// </summary>
public static class UsersModule
{
    /// <summary>
    /// Registers user services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<UsersService>();
    }
}
