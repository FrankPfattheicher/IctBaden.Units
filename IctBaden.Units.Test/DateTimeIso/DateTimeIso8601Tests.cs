using System;
using System.Globalization;
using Xunit;

namespace IctBaden.Units.Test.DateTimeIso
{
    public class DateTimeIso8601Tests
    {
        private readonly DateTimeIso8601 _iso8601;
        private readonly CultureInfo _culture;
        
        public DateTimeIso8601Tests()
        {
            _iso8601 = new DateTimeIso8601();
            _culture = new CultureInfo("de-DE");
        }
        
        [Fact]
        public void FirstFeb2023ShouldBeWeek5()
        {
            var week = _iso8601.GetIso8601WeekOfYear(DateTime.Parse("1.2.2023", _culture));
            
            Assert.Equal(5, week);
        }

        [Fact]
        public void FirstDayOfWeek5ShouldBeJan30()
        {
            var day = _iso8601.FirstDateOfWeekISO8601(2023, 5);
            
            Assert.Equal(DateTime.Parse("30.1.2023", _culture), day);
        }
        
    }
}