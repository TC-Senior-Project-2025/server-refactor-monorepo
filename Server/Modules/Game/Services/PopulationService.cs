using Server.Modules.Game.Core;

namespace Server.Modules.Game.Services;

public static class PopulationService
{
    private const double PopulationGrowth = 0.2 / 100 / 12;

    public static int CalculatePopulationGrowth(Commandery commandery)
    {
        var growth = (float)commandery.Population * PopulationGrowth;
        return (int)growth;
    }
}