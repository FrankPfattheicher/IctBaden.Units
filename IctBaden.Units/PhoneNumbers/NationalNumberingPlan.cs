namespace IctBaden.Units
{
    public class NationalNumberingPlan
    {
        public string IsoName { get; private set; }
        public string CountryCode { get; private set; }

        public NationalNumberingPlan(string isoName, string countryCode)
        {
            IsoName = isoName;
            CountryCode = countryCode;
        }
    }
}