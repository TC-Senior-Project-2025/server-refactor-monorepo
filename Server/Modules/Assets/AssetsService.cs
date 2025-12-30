using System.Text.Json;
using Server.Modules.Assets.Entities;

namespace Server.Modules.Assets;

public class AssetsService(ILogger<AssetsService> logger)
{
    public MapConnections? LoadMapConnections()
    {
        try
        {
            var file = File.ReadAllText("Assets/Data/Map/connections.json");
            var dict = JsonSerializer.Deserialize<Dictionary<int, List<int>>>(file);
            return dict == null ? null : new MapConnections(dict);
        }
        catch (Exception e)
        {
            logger.LogError("Failed to load connections: {Error}", e);
            return null;
        }
    }

    public Scenario? LoadScenario(string scenarioName)
    {
        try
        {
            var file = File.ReadAllText($"Assets/Data/Scenarios/{scenarioName}.json");
            return JsonSerializer.Deserialize<Scenario>(file);
        }
        catch (Exception e)
        {
            logger.LogError("Failed to load scenario: {Error}", e);
            return null;
        }
    }
}