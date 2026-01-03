namespace Server.Modules.Game.Core;

public class NationalResources
{
    public required int Efficiency { get; set; }
    public required int Treasury { get; set; }
    public required int Manpower { get; set; }
    public required int Stability { get; set; }
    public required int Prestige { get; set; }

    public override string ToString()
    {
        return $"[ Efficiency: {Efficiency} | Treasury: {Treasury} | Manpower: {Manpower} | Stability: {Stability} | Prestige: {Prestige} ]";
    }

    public void ApplyChanges(NationalResources other)
    {
        Treasury += other.Treasury;
        Manpower += other.Manpower;
        Stability += other.Stability;
        Prestige += other.Prestige;
        Efficiency += other.Efficiency;
    }

    public NationalResources SetEfficiency(Func<int, int> func)
    {
        Efficiency = func(Efficiency);
        return this;
    }

    public NationalResources SetTreasury(Func<int, int> func)
    {
        Treasury = func(Treasury);
        return this;
    }

    public NationalResources SetManpower(Func<int, int> func)
    {
        Manpower = func(Manpower);
        return this;
    }

    public NationalResources SetStability(Func<int, int> func)
    {
        Stability = func(Stability);
        return this;
    }

    public NationalResources SetPrestige(Func<int, int> func)
    {
        Prestige = func(Prestige);
        return this;
    }

    public static NationalResources Zero()
    {
        return new NationalResources
        {
            Efficiency = 0,
            Treasury = 0,
            Manpower = 0,
            Stability = 0,
            Prestige = 0
        };
    }
}