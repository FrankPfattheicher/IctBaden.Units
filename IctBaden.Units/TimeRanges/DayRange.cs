using System;

namespace IctBaden.Units
{
    public class DayRange
    {
        public TimeRanges Range { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public DayRange(TimeRanges range, DateTime start)
        {
            Range = range;
            Start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            End = Start + TimeSpan.FromDays((int)range) - TimeSpan.FromSeconds(1);
        }
        
    }
}