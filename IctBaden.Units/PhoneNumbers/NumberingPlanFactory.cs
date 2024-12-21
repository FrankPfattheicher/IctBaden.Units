using System.Globalization;

namespace IctBaden.Units;

internal static class NumberingPlanFactory
{
    public static NumberingPlanProvider? GetNumberingPlan(string isoCountryName) =>
        isoCountryName.ToLower(CultureInfo.InvariantCulture) switch
            {
                "de" => new GermanNumberingPlanProvider(),
                _ => null
            };
        
}