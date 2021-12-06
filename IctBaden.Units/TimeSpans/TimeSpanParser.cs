using System;
using System.Text.RegularExpressions;

namespace IctBaden.Units.TimeSpans
{
    public static class TimeSpanParser
    {
        
        public static TimeSpan Parse(string? text)
        {
            var result = new TimeSpan(0);
            if (string.IsNullOrEmpty(text)) return result;
            
            //[ws][-]{ ss | d.hh:mm:ss[.ff] | hh:mm:ss[.ff] }[ws]
            var negative = text!.StartsWith("-");
            if (negative)
            {
                text = text.Substring(1);
            }
            text = text.Trim();

            var formatSeconds = new Regex(@"^([0-9]+)$");
            var match = formatSeconds.Match(text);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out var seconds);
                result = new TimeSpan(0, 0, 0, seconds);
                result = negative ? -result : result;
                return result;
            }
            
            var formatMinSec = new Regex(@"^([0-9]+):([0-9]+)$");
            match = formatMinSec.Match(text);
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out var minutes);
                int.TryParse(match.Groups[2].Value, out var seconds);
                result = new TimeSpan(0, 0, minutes, seconds);
                result = negative ? -result : result;
                return result;
            }

            //                                  12           34         5        6  7       8  9
            var formatFull = new Regex(@"^(([0-9]+)\.)?(([0-9]+)\:([0-9]+))(\:([0-9]+)(\.([0-9]+))?)?$");
            match = formatFull.Match(text);
            if (match.Success)
            {
                int.TryParse(match.Groups[2].Value, out var days);
                int.TryParse(match.Groups[4].Value, out var hours);
                int.TryParse(match.Groups[5].Value, out var minutes);
                int.TryParse(match.Groups[7].Value, out var seconds);
                int.TryParse(match.Groups[9].Value, out var fraction);
                result = new TimeSpan(days, hours, minutes, seconds, fraction);
                result = negative ? -result : result;
                return result;
            }
            return result;
        }
        
    }
}