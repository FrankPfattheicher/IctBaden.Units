namespace IctBaden.Units
{
    internal static class NumberingPlanFactory
    {
        public static NumberingPlanProvider? GetNumberingPlan(string? isoCountryName) =>
            string.IsNullOrEmpty(isoCountryName)
                ? null
                : isoCountryName.ToUpper() switch
                {
                    "DE" => new GermanNumberingPlan(),
                    _ => null
                };
        
    }
}