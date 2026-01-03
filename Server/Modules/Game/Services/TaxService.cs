using Server.Modules.Game.Core;

namespace Server.Modules.Game.Services;

public static class TaxService
{
    private const float TaxDivisor = 12 * 100000;

    public static int CalculateTax(float efficiency, List<Commandery> commanderies)
    {
        var tax = efficiency * commanderies
            .Select(p =>
            {
                var unrestTaxBurden = 1.0f;
                if (p.Unrest > 5)
                {
                    unrestTaxBurden = (15 - p.Unrest) / 10.0f;
                }
                var tax = (p.Population * p.Wealth / TaxDivisor) * unrestTaxBurden;
                return tax;
            })
            .Sum();
        return (int)tax;
    }
}