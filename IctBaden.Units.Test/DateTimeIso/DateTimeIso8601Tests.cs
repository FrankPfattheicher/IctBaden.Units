using System;
using Xunit;

namespace IctBaden.Units.Test.DateTimeIso
{
    public class DateTimeIso8601Tests
    {
        private readonly Units.DateTimeIso8601 _iso8601;
        
        public DateTimeIso8601Tests()
        {
            _iso8601 = new Units.DateTimeIso8601();
        }
        
        [Fact]
        public void FirstFeb2023ShouldBeWeek5()
        {
            var week = _iso8601.GetIso8601WeekOfYear(DateTime.Parse("1.2.2023"));
            
            Assert.Equal(5, week);
        }

        [Fact]
        public void FirstDayOfWeek5ShouldBeJan30()
        {
            var day = _iso8601.FirstDateOfWeekISO8601(2023, 5);
            
            Assert.Equal(DateTime.Parse("30.1.2023"), day);
        }
        
    }
}