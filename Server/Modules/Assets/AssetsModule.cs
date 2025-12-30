namespace Server.Modules.Assets;

public static class AssetsModule
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<AssetsService>();
    }
}