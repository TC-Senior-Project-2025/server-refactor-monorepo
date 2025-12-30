namespace Server.Modules.Commanderies;

public class CommanderiesModule
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<CommanderiesService>();
    }
}