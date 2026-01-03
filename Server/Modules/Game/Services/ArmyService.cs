using Server.Modules.Game.Core;

namespace Server.Modules.Game.Services;

public static class ArmyService
{
    public const float ManpowerDivisor = 12 * 100;
    public const float ArmyUpkeep = 0.00025f;
    public const int SupplyDecay = 30;
    public const int Attrition = 10;

    public static int CalculateManpowerGain(float efficiency, Commandery commandery)
    {
        var manpower = efficiency
            * commandery.Population / ManpowerDivisor
            * (12 - commandery.Unrest) / 10.0f;
        return (int)manpower;
    }

    public static int CalculateUnitCost(Unit unit)
    {
        return (int)(unit.Size * ArmyUpkeep);
    }
}
