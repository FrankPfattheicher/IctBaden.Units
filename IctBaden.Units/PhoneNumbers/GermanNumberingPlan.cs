using System.Collections.Generic;
using System.IO;
using IctBaden.Framework.Resource;

namespace IctBaden.Units;

internal static class GermanNumberingPlan
{
    public static readonly NumberingPlanEntry[] CodeList;

    static GermanNumberingPlan()
    {
        var codes = new List<NumberingPlanEntry>();
        var germanNumberingPlanDef = ResourceLoader.LoadString("GermanNumberingPlan.tsv") ?? string.Empty;
        using (var loader = new StringReader(germanNumberingPlanDef))
        {
            while (true)
            {
                var line = loader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;
                var entry = line.Split('\t');
                if (entry.Length == 2)
                    codes.Add(new NumberingPlanEntry(entry[0], entry[1]));
            }
        }

        CodeList = codes.ToArray();
    }
}