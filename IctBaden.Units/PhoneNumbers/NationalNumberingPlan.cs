// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace IctBaden.Units
{
    public class NationalNumberingPlan
    {
        /// <summary>
        /// Two character ISO code
        /// </summary>
        public string IsoName { get; private set; }
        /// <summary>
        /// E.164 numeric country code
        /// </summary>
        public string CountryCode { get; private set; }

        public NationalNumberingPlan(string isoName, string countryCode)
        {
            IsoName = isoName;
            CountryCode = countryCode;
        }
    }
}