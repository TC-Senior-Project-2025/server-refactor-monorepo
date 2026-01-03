using Server.Modules.Game.Core;

namespace Server.Modules.Game.Services;

public static class TurnService
{
    public class TurnProcessResult
    {
        public int TaxRevenue { get; set; }
        public int ManpowerGrowth { get; set; }
        public int PopulationGrowth { get; set; }
        public int ArmyExpenses { get; set; }
    }

    public static TurnProcessResult ProcessTurn(GameState gameState)
    {
        var result = new TurnProcessResult();
        Random rng = new();

        foreach (var country in gameState.Countries.Values)
        {
            var commanderies = gameState.Commanderies.Values.Where(p => p.CountryId == country.Id).ToList();

            // Calculate tax from commanderies
            var tax = TaxService.CalculateTax(country.Resources.Efficiency, commanderies);
            country.Resources.SetTreasury(t => t + tax);

            if (country.Id == gameState.PlayerCountryId)
            {
                result.TaxRevenue = tax;
            }

            foreach (var commandery in commanderies)
            {
                // Calculate manpower growth
                var manpowerGrowth = ArmyService.CalculateManpowerGain(country.Resources.Efficiency, commandery);
                country.Resources.SetManpower(m => m + manpowerGrowth);
                commandery.Population -= manpowerGrowth;

                // Calculate population growth
                var populationGrowth = PopulationService.CalculatePopulationGrowth(commandery);
                commandery.Population += populationGrowth;

                if (country.Id == gameState.PlayerCountryId)
                {
                    result.ManpowerGrowth += manpowerGrowth;
                    result.PopulationGrowth += populationGrowth;
                }
            }

            // Efficiency decay
            if (country.Resources.Efficiency > 100)
            {
                country.Resources.Efficiency -= 2;
            }
            else if (country.Resources.Efficiency > 90)
            {
                country.Resources.Efficiency -= 1;
            }

            // Stability decay
            if (country.Resources.Stability > 100)
            {
                country.Resources.Stability -= 1;
            }
            else if (country.Resources.Stability < 50)
            {
                country.Resources.Stability += 1;
            }

            // Prestige decay
            if (country.Resources.Prestige > country.Resources.Stability)
            {
                country.Resources.Prestige -= 2;
            }
            else if (country.Resources.Prestige > country.Resources.Stability / 2)
            {
                country.Resources.Prestige -= 1;
            }
        }

        foreach (var unit in gameState.Units)
        {
            var country = gameState.Countries[unit.CountryId];
            var unitCost = ArmyService.CalculateUnitCost(unit);
            country.Resources.SetTreasury(t => t - unitCost);

            if (country.Id == gameState.PlayerCountryId)
            {
                result.ArmyExpenses += unitCost;
            }

            // TODO: Implement max morale logic for units with commanders

            var maxMorale = 90 + (int)(country.Resources.Prestige / 10.0);
            if (unit.Morale > maxMorale)
            {
                unit.Morale = Math.Max(maxMorale, unit.Morale - 5);
            }

            var location = gameState.Commanderies[unit.LocationId];
            if (location.CountryId == unit.CountryId)
            {
                // If in allied territory
                var moraleGain = 10 + (int)(country.Resources.Prestige / 10.0);
                unit.Supply = Math.Clamp(unit.Supply + 10, 0, 100);
                unit.Morale = Math.Clamp(unit.Morale + moraleGain, 0, maxMorale);
            }
            else
            {
                // If in enemy territory: supply decay
                unit.Supply = Math.Clamp(unit.Supply - ArmyService.SupplyDecay, 0, 100);
                unit.Size = (int)(unit.Size * rng.Next(100 - ArmyService.Attrition, 101) / 100.0);
            }

            if (unit.Supply <= 0)
            {
                unit.Morale = Math.Max(unit.Morale + (int)(unit.Supply / 2.0), 0);
                unit.Size = (int)(unit.Size * (100 + unit.Supply / 2.0) / 100.0);
                unit.Supply = 0;
            }
        }

        // Destroy all units with size = 0
        var destroyedUnits = gameState.Units.Where(u => u.Size <= 0).ToList();
        gameState.Units.RemoveAll(u => u.Size <= 0);

        return result;
    }
}