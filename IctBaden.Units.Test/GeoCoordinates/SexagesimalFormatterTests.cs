using Xunit;

namespace IctBaden.Units.Test.GeoCoordinates
{
    public class SexagesimalFormatterTests
    {
        const string Text = " 32° 8.385' N ";
        private readonly SexagesimalCoordinate _coordinate;

        public SexagesimalFormatterTests()
        {
            _coordinate = SexagesimalCoordinateParser.Parse(Text);
        }
        
        [Fact]
        public void DefaultFormatShouldBeDecimal()
        {
            var formatted = _coordinate.ToLatString();
            Assert.StartsWith("32,1355", formatted);
        }

        [Fact]
        public void Format_g_ShouldBeDegrees()
        {
            var formatted = _coordinate.ToLatString("g");
            Assert.Equal("32° 8' 7,87\" N", formatted);
        }

        [Fact]
        public void Format_g00_ShouldBeDegrees()
        {
            var formatted = _coordinate.ToLatString("g00");
            Assert.Equal("32° 08' 07,87\" N", formatted);
        }

    }
}