using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using IctBaden.Framework.Resource;

namespace IctBaden.Units
{
    internal class GermanNumberingPlan : NumberingPlanProvider
    {
        private static readonly object CodeListLock = new object();

        private static List<NumberingPlanEntry> _codeList;
        public override List<NumberingPlanEntry> CodeList
        {
            get
            {
                lock (CodeListLock)
                {
                    if (_codeList != null)
                        return _codeList;

                    _codeList = new List<NumberingPlanEntry>();
                    var germanNumberingPlanDef = ResourceLoader.LoadString("GermanNumberingPlan.tsv");
                    using (var loader = new StringReader(germanNumberingPlanDef))
                    {
                        while (true)
                        {
                            var line = loader.ReadLine();
                            if (string.IsNullOrEmpty(line))
                                break;
                            var entry = line.Split('\t');
                            if (entry.Length == 2)
                                _codeList.Add(new NumberingPlanEntry(entry[0], entry[1]));
                        }
                    }
                    return _codeList;
                }
            }
        }

        public override string ResolveInternationalDialling(string text)
        {
            var isInternationalDialling = new Regex("^ *0 *0 *(.*)$");
            var match = isInternationalDialling.Match(text);
            if (!match.Success)
                return text;

            return "+" + match.Groups[1].Value.Trim();
        }

        public override bool Parse(ref PhoneNumber number, ref string text)
        {
            var detectAreaCode = new Regex(@"^\(([0-9 ]+)\)").Match(text);
            if (detectAreaCode.Success)
            {
                if (detectAreaCode.Groups[1].Value == "0")
                {
                    text = PhoneNumber.Trim(text.Replace("(0)", ""));
                }
            }

            text = PhoneNumber.Trim(text);
            if (string.IsNullOrEmpty(number.CountryCode))
            {
                if (!text.StartsWith("0"))
                {
                    return false;
                }
                text = text.Substring(1);
            }

            foreach (var entry in CodeList)
            {
                var match = entry.Validator.Match(text);
                if (!match.Success)
                    continue;
                text = text.Substring(match.Groups[0].Value.Length);
                while ((text.Length > 0) && !char.IsDigit(text[0]))
                {
                    text = text.Substring(1);
                }
                number.AreaCode = entry.Code;
                number.AreaName = entry.Description;
                return true;
            }

            return false;
        }
    }
}
