using System.Globalization;
using System.Text.RegularExpressions;

namespace IctBaden.Units;

public static class SexagesimalCoordinateParser
{
    public static SexagesimalCoordinate Parse(string text)
    {
        text = text
            .Trim()
            .ToUpper()
            .Replace(",", ".");
            
        // -32° 18.385'
        var match = new Regex(@"^([+-]?[0-9]+)\s*°\s*([0-9]+(\.([0-9]+))?)\s*'$").Match(text);
        if (match.Success)
        {
            var degrees = int.Parse(match.Groups[1].Value, NumberStyles.Integer | NumberStyles.AllowLeadingSign);
            var minutes = double.Parse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            var coordinate = new SexagesimalCoordinate(degrees, minutes);
            return coordinate;
        }
            
        // 32° 18.385' N
        match = new Regex(@"^([+-]?[0-9]+)\s*°\s*([0-9]+(\.([0-9]+))?)\s*'\s*([NSOW])$").Match(text);
        if (match.Success)
        {
            var degrees = int.Parse(match.Groups[1].Value, NumberStyles.Integer | NumberStyles.AllowLeadingSign);
            var minutes = double.Parse(match.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            if ("SW".Contains(match.Groups[5].Value))
            {
                degrees = -degrees;
            }

            var coordinate = new SexagesimalCoordinate(degrees, minutes)
            {
                Type = "NS".Contains(match.Groups[5].Value)
                    ? SexagesimalCoordinate.CoordinateType.Latitude
                    : SexagesimalCoordinate.CoordinateType.Longitude
            };
            return coordinate;
        }
            
        // 37°25'19.07"N
        match = new Regex("^([0-9]+)\\s*°\\s*([0-9]+)\\s*'\\s*([0-9]+(\\.([0-9]+))?)\\s*\"\\s*([NSOW])$").Match(text);
        if (match.Success)
        {
            var degrees = int.Parse(match.Groups[1].Value, NumberStyles.Integer | NumberStyles.AllowLeadingSign);
            var minutes = int.Parse(match.Groups[2].Value, NumberStyles.Integer);
            var seconds = double.Parse(match.Groups[3].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            if ("SW".Contains(match.Groups[6].Value))
            {
                degrees = -degrees;
            }

            var coordinate = new SexagesimalCoordinate(degrees, minutes, seconds)
            {
                Type = "NS".Contains(match.Groups[6].Value)
                    ? SexagesimalCoordinate.CoordinateType.Latitude
                    : SexagesimalCoordinate.CoordinateType.Longitude
            };
            return coordinate;
        }
            
        // -37.7°
        match = new Regex("^([+-]?[0-9]+(\\.([0-9]+))?)\\s*°?$").Match(text);
        if (match.Success)
        {
            var degrees = double.Parse(match.Groups[1].Value, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            var coordinate = new SexagesimalCoordinate(degrees);
            return coordinate;
        }

        // 37.7° S
        match = new Regex("^([0-9]+(\\.([0-9]+))?)\\s*°?\\s*([NSOW])$").Match(text);
        if (match.Success)
        {
            var degrees = double.Parse(match.Groups[1].Value, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            if ("SW".Contains(match.Groups[4].Value))
            {
                degrees = -degrees;
            }

            var coordinate = new SexagesimalCoordinate(degrees)
            {
                Type = "NS".Contains(match.Groups[4].Value)
                    ? SexagesimalCoordinate.CoordinateType.Latitude
                    : SexagesimalCoordinate.CoordinateType.Longitude
            };
            return coordinate;
        }

        // 37 25 19.07
        match = new Regex("^([0-9]+)\\s*([0-9]+)\\s*([0-9]+(\\.([0-9]+))?)\\s*([NSOW])?$").Match(text);
        if (match.Success)
        {
            var degrees = int.Parse(match.Groups[1].Value, NumberStyles.Integer | NumberStyles.AllowLeadingSign);
            var minutes = int.Parse(match.Groups[2].Value, NumberStyles.Integer);
            var seconds = double.Parse(match.Groups[3].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(match.Groups[6].Value) && "SW".Contains(match.Groups[6].Value))
            {
                degrees = -degrees;
            }

            var coordinate = new SexagesimalCoordinate(degrees, minutes, seconds)
            {
                Type = "NS".Contains(match.Groups[6].Value)
                    ? SexagesimalCoordinate.CoordinateType.Latitude
                    : SexagesimalCoordinate.CoordinateType.Longitude
            };
            return coordinate;
        }

        return new SexagesimalCoordinate(0.0);
    }
}