using System;
using System.Linq;

// ReSharper disable CommentTypo

namespace IctBaden.Units
{
    internal static class InternationalNumberingPlan
    {
        private static NationalNumberingPlan[] _codeList;

        private static NationalNumberingPlan[] CodeList
        {
            get
            {
                if (_codeList != null)
                    return _codeList;

                _codeList = new NationalNumberingPlan[]
                    //   E.164 numeric country code  iso-name
                    {
                        new NationalNumberingPlan("DE", "49"),
                        new NationalNumberingPlan("UK", "44"),
                        new NationalNumberingPlan("NANP", "1")  // Nordamerikanischer Nummerierungsplan
                    };
                return _codeList;
            }
        }

        public static string GetCountryCodeByName(string name) => CodeList
            .FirstOrDefault(c => string.Compare(c.IsoName, name, StringComparison.InvariantCultureIgnoreCase) == 0)?.CountryCode ?? string.Empty;

        public static string GetNameByCountryCode(string countryCode) => CodeList
            .FirstOrDefault(c => c.CountryCode == countryCode)?.IsoName ?? string.Empty;

        public static bool Parse(ref PhoneNumber number, ref string text)
        {
            if (!text.StartsWith("+"))
                return false;

            for (var ix = 0; ix < CodeList.GetLength(0); ix++)
            {
                var numberingPlan = CodeList[ix];
                if (!text.StartsWith(numberingPlan.CountryCode))
                    continue;

                text = text.Substring(numberingPlan.CountryCode.Length).Trim();
                number.CountryCode = numberingPlan.CountryCode;
                number.CountryName = numberingPlan.IsoName;
                return true;
            }

            return false;
        }
    }
}