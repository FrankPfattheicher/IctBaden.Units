using System;
using System.Globalization;

namespace IctBaden.Units
{
    public class DateTimeIso8601
    {
        private readonly Calendar _calendar;
        private readonly CalendarWeekRule _weekRule;

        public DateTimeIso8601()
            : this(CultureInfo.CurrentCulture.Calendar, CalendarWeekRule.FirstFourDayWeek)
        {
        }

        public DateTimeIso8601(Calendar calendar, CalendarWeekRule weekRule)
        {
            _calendar = calendar;
            _weekRule = weekRule;
        }

        /// <summary>
        /// This presumes that weeks start with Monday, which is the first weekday in ISO8601
        /// Week 1 is the 1st week of the year with a Thursday in it. 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            var day = _calendar.GetDayOfWeek(time);
            if (day is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return _calendar.GetWeekOfYear(time, _weekRule, DayOfWeek.Monday);
        }

        /// <summary>
        /// Returns first day of given week of year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        public DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            var firstThursday = jan1.AddDays(daysOffset);
            var firstWeek = GetIso8601WeekOfYear(firstThursday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
        
        
        
    }
}