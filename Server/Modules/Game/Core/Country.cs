using Server.Modules.Countries.Entities;

namespace Server.Modules.Game.Core;

public class Country
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public int Efficiency { get; set; } = 50;
    public int Treasury { get; set; } = 50;
    public int Stability { get; set; } = 50;
    public int Manpower { get; set; } = 50;
    public int Prestige { get; set; } = 50;

}