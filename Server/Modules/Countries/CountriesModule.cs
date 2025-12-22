namespace Server.Modules.Countries;

public static class CountriesModule
{
    /// <summary>
    /// Registers country services with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<CountriesService>();
    }
}