using System.Globalization;
using Xunit;

namespace IctBaden.Units.Test.GeoCoordinates
{
    public class SexagesimalFormatterTests
    {
        private readonly CultureInfo _culture;

        private const string TextLatitude = " 32° 8.385' N ";
        private const string TextLongitude = " 5° 49.5' O ";

        private readonly SexagesimalCoordinate _latitude;
        private readonly SexagesimalCoordinate _longitude;

        public SexagesimalFormatterTests()
        {
            _latitude = SexagesimalCoordinateParser.Parse(TextLatitude);
            _longitude = SexagesimalCoordinateParser.Parse(TextLongitude);
            _culture = new CultureInfo("de-DE");
        }
        
        
        [Fact]
        public void LatitudeDefaultFormatShouldBeDecimal()
        {
            var formatted = _latitude.ToLatString(_culture);
            Assert.StartsWith("32,1355", formatted);
        }

        [Fact]
        public void LatitudeFormat_g_ShouldBeDegrees()
        {
            var formatted = _latitude.ToLatString(_culture, "g");
            Assert.Equal("32° 8' 7,87\" N", formatted);
        }

        [Fact]
        public void LatitudeFormat_g00_ShouldBeDegrees()
        {
            var formatted = _latitude.ToLatString(_culture, "g00");
            Assert.Equal("32° 08' 07,87\" N", formatted);
        }
        
        
        [Fact]
        public void LongitudeDefaultFormatShouldBeDecimal()
        {
            var formatted = _longitude.ToLongString(_culture);
            Assert.StartsWith("5,8300", formatted);
        }

        [Fact]
        public void LongitudeFormat_g_ShouldBeDegrees()
        {
            var formatted = _longitude.ToLongString(_culture, "g");
            Assert.Equal("5° 49' 48,18\" O", formatted);
        }

        [Fact]
        public void LongitudeFormat_g00_ShouldBeDegrees()
        {
            var formatted = _longitude.ToLongString(_culture, "g00");
            Assert.Equal("05° 49' 48,18\" O", formatted);
        }

    }
}