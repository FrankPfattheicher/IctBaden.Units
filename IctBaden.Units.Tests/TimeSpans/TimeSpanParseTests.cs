using IctBaden.Units.TimeSpans;
using Xunit;

namespace IctBaden.Units.Tests.TimeSpans
{
    public class TimeSpanParseTests
    {
        
        [Fact]
        public void InvalidTextShouldParsedAsTimeSpanNull()
        {
            const string number = "invalid";
            var timeSpan = TimeSpanParser.Parse(number);
            Assert.Equal(0, timeSpan.Ticks);
        }

        [Fact]
        public void SimpleNumberShouldParsedAsSeconds()
        {
            const string number = "26";
            var timeSpan = TimeSpanParser.Parse(number);
            Assert.Equal(26, timeSpan.TotalSeconds);
        }
        
        [Fact]
        public void MinutesAndSecondsShouldParsed()
        {
            const string number = "1:23";
            var timeSpan = TimeSpanParser.Parse(number);
            Assert.Equal(60 + 23, timeSpan.TotalSeconds);
        }

        [Fact]
        public void NegativeMinutesAndSecondsShouldParsed()
        {
            const string number = "-1:23";
            var timeSpan = TimeSpanParser.Parse(number);
            Assert.Equal(-(60 + 23), timeSpan.TotalSeconds);
        }

        [Fact]
        public void HoursMinutesSecondsShouldParsed()
        {
            const string number = "2:30:23";
            var timeSpan = TimeSpanParser.Parse(number);
            Assert.Equal((2 * 60 * 60) + (30 * 60) + 23, timeSpan.TotalSeconds);
        }

        [Fact]
        public void DaysHoursMinutesSecondsShouldParsed()
        {
            const string number = "7.08:45:58";
            var timeSpan = TimeSpanParser.Parse(number);
            var seconds = 7 * (24 * 60 * 60);
            seconds += 8 * (60 * 60);
            seconds += 45 * 60;
            seconds += 58;
            Assert.Equal(seconds, timeSpan.TotalSeconds);
        }

    }
}