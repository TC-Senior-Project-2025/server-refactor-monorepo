using Server.Modules.Countries.Entities;

namespace Server.Modules.Game.Core;

public class Country
{
    public required string Code { get; set; }
    public required string Name { get; set; }
   
    public required NationalResources Resources { get; set; }
    
    public string? RecentSituationSummary { get; set; }
}