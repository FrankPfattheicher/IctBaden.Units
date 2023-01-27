using System;
using Xunit;

namespace IctBaden.Units.Test.TimeRanges
{
    public class DayRangeTests
    {
        [Fact]
        public void OneDayRangeShouldBe00h00To23h59()
        {
            var now = DateTime.Now;
            var range = new DayRange(Units.TimeRanges.Day, now);

            Assert.Equal(now.Year, range.Start.Year);
            Assert.Equal(now.Month, range.Start.Month);
            Assert.Equal(now.Day, range.Start.Day);

            Assert.Equal(0, range.Start.Hour);
            Assert.Equal(0, range.Start.Minute);
            Assert.Equal(0, range.Start.Second);
            
            Assert.Equal(now.Year, range.End.Year);
            Assert.Equal(now.Month, range.End.Month);
            Assert.Equal(now.Day, range.End.Day);

            Assert.Equal(23, range.End.Hour);
            Assert.Equal(59, range.End.Minute);
            Assert.Equal(59, range.End.Second);
        }
    }
}