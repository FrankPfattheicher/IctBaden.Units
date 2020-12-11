namespace IctBaden.Units
{
    internal static class InternationalNumberingPlan
    {
        private static string[,] _codeList;

        private static string[,] CodeList
        {
            get
            {
                if (_codeList != null)
                    return _codeList;

                _codeList = new[,]
                    //   code  iso-name
                    {
                        {"+49", "DE"},
                        {"+44", "UK"},
                        {"+1", "NANP"}  // Nordamerikanischer Nummerierungsplan
                    };
                return _codeList;
            }
        }

        public static string GetCountryCodeByName(string name)
        {
            for (var ix = 0; ix < CodeList.GetLength(0); ix++)
            {
                var country = CodeList[ix, 1];
                if (country == name)
                    return CodeList[ix, 0].Substring(1);
            }

            return string.Empty;
        }

        public static string GetNameByCountryCode(string countryCode)
        {
            for (var ix = 0; ix < CodeList.GetLength(0); ix++)
            {
                var code = CodeList[ix, 0].Substring(1);
                if (code == countryCode)
                    return CodeList[ix, 1];
            }

            return string.Empty;
        }

        public static bool Parse(ref PhoneNumber number, ref string text)
        {
            if (!text.StartsWith("+"))
                return false;

            for (var ix = 0; ix < CodeList.GetLength(0); ix++)
            {
                var code = CodeList[ix, 0];
                if (!text.StartsWith(code))
                    continue;

                text = text.Substring(code.Length).Trim();
                number.CountryCode = code.Substring(1);
                number.CountryName = CodeList[ix, 1];
                return true;
            }

            return false;
        }
    }
}