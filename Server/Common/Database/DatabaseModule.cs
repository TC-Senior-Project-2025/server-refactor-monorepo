using Microsoft.EntityFrameworkCore;

namespace Server.Common.Database;

/// <summary>
/// Module for registering database services.
/// </summary>
public static class DatabaseModule
{
    /// <summary>
    /// Registers database services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
