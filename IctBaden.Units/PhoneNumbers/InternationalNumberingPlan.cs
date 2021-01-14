using System;
using System.Linq;

// ReSharper disable CommentTypo

namespace IctBaden.Units
{
    internal static class InternationalNumberingPlan
    {
        private static NationalNumberingPlan[]? _nationalPlans;

        private static NationalNumberingPlan[] NationalPlans
        {
            get
            {
                if (_nationalPlans != null)
                    return _nationalPlans;

                _nationalPlans = new[]
                    //   E.164 numeric country code  iso-name
                    {
                        new NationalNumberingPlan("DE", "49"),
                        new NationalNumberingPlan("UK", "44"),
                        new NationalNumberingPlan("FR", "33"),
                        new NationalNumberingPlan("NANP", "1")  // Nordamerikanischer Nummerierungsplan
                    };
                return _nationalPlans;
            }
        }

        public static string GetCountryCodeByName(string? name) => NationalPlans
            .FirstOrDefault(c => string.Compare(c.IsoName, name, StringComparison.InvariantCultureIgnoreCase) == 0)?.CountryCode ?? string.Empty;

        public static string GetNameByCountryCode(string? countryCode) => NationalPlans
            .FirstOrDefault(c => c.CountryCode == countryCode)?.IsoName ?? string.Empty;

        public static PhoneNumber? Parse(ref string text)
        {
            if (!text.StartsWith("+"))
                return null;
            
            text = text.Substring(1);
            foreach (var numberingPlan in NationalPlans)
            {
                if (!text.StartsWith(numberingPlan.CountryCode))
                    continue;

                text = text.Substring(numberingPlan.CountryCode.Length).Trim();
                return new PhoneNumber(numberingPlan.CountryCode, null, null, null, false)
                    {CountryName = numberingPlan.IsoName};
            }

            return null;
        }
    }
}