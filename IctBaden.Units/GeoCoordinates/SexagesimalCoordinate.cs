// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System;

namespace IctBaden.Units
{
    public class SexagesimalCoordinate
    {
        public enum CoordinateType
        {
            Undefined,
            Longitude,
            Latitude
        }


        public CoordinateType Type { get; set; } = CoordinateType.Undefined;
        public int Degrees { get; set; }
        public int Minutes { get; set; }
        public double Seconds { get; set; }

        public double DecimalValue => Degrees >= 0
            ? Degrees
              + Minutes / 60.0
              + Seconds / 3600.0
            : Degrees
              - Minutes / 60.0
              - Seconds / 3600.0;


        // ReSharper disable once UnusedMember.Global
        public SexagesimalCoordinate()
        {
        }

        public SexagesimalCoordinate(int degrees, int minutes, double seconds)
        {
            Degrees = degrees;
            Minutes = minutes;
            Seconds = seconds;
        }

        public SexagesimalCoordinate(int degrees, double minutes)
        {
            Degrees = degrees;
            Minutes = Math.Abs((int) minutes);
            Seconds = Minutes - Minutes / 60.0;
        }

        public SexagesimalCoordinate(double decimalValue)
        {
            var value = Math.Abs(decimalValue);
            Degrees = (int) value;
            Minutes = (int) ((value - Degrees) * 60);
            Seconds = (value - Degrees - Minutes / 60.0) * 3600;
            Degrees = (int) decimalValue;
        }
    }
}