namespace IctBaden.Units;

internal abstract class NumberingPlanProvider
{
    public abstract NumberingPlanEntry[] CodeList { get; }
        
    public abstract bool Parse(ref PhoneNumber number, ref string text);

    public virtual string ResolveInternationalDialling(string text) => text;
}