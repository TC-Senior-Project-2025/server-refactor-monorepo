namespace Server.Modules.Scenarios;

public static class ScenariosModule
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ScenariosService>();
    }
}