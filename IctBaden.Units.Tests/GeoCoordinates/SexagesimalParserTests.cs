using System.Globalization;
using Xunit;

namespace IctBaden.Units.Tests.GeoCoordinates
{
    
    public class SexagesimalParserTests
    {
        [Fact]
        public void DecimalGradShouldBeParsable()
        {
            const string text = " 32° 18.385' N ";

            var coordinate = SexagesimalCoordinateParser.Parse(text);
        
            Assert.Equal(32, coordinate.Degrees);
            Assert.Equal(18, coordinate.Minutes);
            Assert.Equal(17, (int)coordinate.Seconds);
            Assert.Equal(SexagesimalCoordinate.CoordinateType.Latitude, coordinate.Type);
        }
        
        [Fact]
        public void GradMinutesSecondsShouldBeParsable()
        {
            const string text = " 37° 25' 19.07\" N ";

            var coordinate = SexagesimalCoordinateParser.Parse(text);
        
            Assert.Equal(37, coordinate.Degrees);
            Assert.Equal(25, coordinate.Minutes);
            Assert.Equal(19, (int)coordinate.Seconds);
            Assert.Equal(SexagesimalCoordinate.CoordinateType.Latitude, coordinate.Type);
        }

        [Fact]
        public void GradMinutesSecondsWithoutSpaceShouldBeParsable()
        {
            const string text = "37°25'19.07\"N";

            var coordinate = SexagesimalCoordinateParser.Parse(text);
        
            Assert.Equal(37, coordinate.Degrees);
            Assert.Equal(25, coordinate.Minutes);
            Assert.Equal(19, (int)coordinate.Seconds);
            Assert.Equal(SexagesimalCoordinate.CoordinateType.Latitude, coordinate.Type);
        }

        [Fact]
        public void DecimalGradWithSignShouldBeParsable()
        {
            var source = new SexagesimalCoordinate(-37, 25, 19.07);
            var text = source.DecimalValue.ToString(CultureInfo.InvariantCulture);

            var coordinate = SexagesimalCoordinateParser.Parse(text);
        
            Assert.Equal(-37, coordinate.Degrees);
            Assert.Equal(25, coordinate.Minutes);
            Assert.Equal(19, (int)coordinate.Seconds);
            Assert.Equal(SexagesimalCoordinate.CoordinateType.Undefined, coordinate.Type);
        }

        [Fact]
        public void DecimalGradWithTypeShouldBeParsable()
        {
            var source = new SexagesimalCoordinate(37, 25, 19.07);
            var text = source.DecimalValue.ToString(CultureInfo.InvariantCulture) + "O";

            var coordinate = SexagesimalCoordinateParser.Parse(text);
        
            Assert.Equal(37, coordinate.Degrees);
            Assert.Equal(25, coordinate.Minutes);
            Assert.Equal(19, (int)coordinate.Seconds);
            Assert.Equal(SexagesimalCoordinate.CoordinateType.Longitude, coordinate.Type);
        }

        
    }
    
}