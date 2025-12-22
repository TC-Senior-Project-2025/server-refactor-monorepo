namespace Server.Modules.Auth;

/// <summary>
/// Module for registering authentication-related services.
/// </summary>
public static class AuthModule
{
    /// <summary>
    /// Registers authentication services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<AuthService>();
    }
}
