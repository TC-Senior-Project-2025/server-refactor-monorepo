namespace Server.Modules.Game.Core;

public class Country
{
    public required int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
   
    public required NationalResources Resources { get; set; }
    
    public string? RecentSituationSummary { get; set; }
}