using System.Text.Json;
using Server.Modules.Scenarios.Entities;

namespace Server.Modules.Scenarios;

public class ScenariosService
{
    public Scenario? LoadScenario(string scenarioName)
    {
        var file = File.ReadAllText($"Assets/Data/{scenarioName}.json");
        return JsonSerializer.Deserialize<Scenario>(file);
    }
}