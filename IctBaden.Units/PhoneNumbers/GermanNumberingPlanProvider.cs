using System.Text.RegularExpressions;

namespace IctBaden.Units;

internal class GermanNumberingPlanProvider : NumberingPlanProvider
{
    public override NumberingPlanEntry[] CodeList => GermanNumberingPlan.CodeList;

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
            if (string.Equals(detectAreaCode.Groups[1].Value, "0", System.StringComparison.OrdinalIgnoreCase))
            {
                text = PhoneNumber.Trim(text.Replace("(0)", ""));
            }
        }

        text = PhoneNumber.Trim(text);
        if (string.IsNullOrEmpty(number.CountryCode))
        {
            if (!text.StartsWith('0'))
            {
                return false;
            }
            text = text.Substring(1);
        }

        foreach (var entry in GermanNumberingPlan.CodeList)
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