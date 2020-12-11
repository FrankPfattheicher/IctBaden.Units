namespace IctBaden.Units
{
    internal abstract class NumberingPlanProvider
    {
        public abstract bool Parse(ref PhoneNumber number, ref string text);

        public virtual void ResolveInternationalDialling(ref PhoneNumber number, ref string text)
        {
        }
    }
}