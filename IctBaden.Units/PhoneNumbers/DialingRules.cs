using System;

namespace IctBaden.Units;

public static class DialingRules
{
    public const string Internal = "G";
    public const string Local = "G";
    public const string LongDistance = "0FG";
    public const string International = "+EFG";

    public static string FromDialType(DialTypes dialType) =>
        dialType switch
        {
            DialTypes.Internal => Internal,
            DialTypes.Local => Local,
            DialTypes.LongDistance => LongDistance,
            DialTypes.International => International,
            _ => International
        };

}
