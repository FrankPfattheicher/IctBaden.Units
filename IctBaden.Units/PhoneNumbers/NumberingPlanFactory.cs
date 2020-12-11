namespace IctBaden.Units
{
    internal static class NumberingPlanFactory
    {
        public static NumberingPlanProvider GetNumberingPlan(string isoNumberingPlanCountryCode)
        {
            switch (isoNumberingPlanCountryCode.ToUpper())
            {
                case "DE":
                    return new GermanNumberingPlan();
            }

            return null;
        }
    }
}