using System.Text;
using System.Text.RegularExpressions;

namespace IctBaden.Units
{
    internal class NumberingPlanEntry
    {
        public string Code { get; private set; }
        public string Description { get; private set; }
        public Regex Validator { get; private set; }

        public NumberingPlanEntry(string code, string description)
        {
            Code = code;
            Description = description;

            var pattern = new StringBuilder();
            pattern.Append("^( *");
            foreach (var digit in code)
            {
                pattern.Append(digit);
                pattern.Append(" *");
            }

            pattern.Append(")");
            Validator = new Regex(pattern.ToString());
        }
    }
}