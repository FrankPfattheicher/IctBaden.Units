using System;
// ReSharper disable CommentTypo

namespace IctBaden.Units
{
    public static class SexagesimalCoordinateFormatter
    {
        /// <summary>
        /// Format the given latitude
        /// Possible formats:
        /// d - Dezimalgrad, zum Beispiel 37.7°
        /// g - Grad, Minuten, Sekunden, zum Beispiel 37°25'19.07"N
        /// m - Grad, Dezimalminuten, zum Beispiel 32° 18.385' N
        ///     Grad, Dezimalgrad ohne Grad-Zeichen
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="format">Format to be used: d, g or m</param>
        /// <returns></returns>
        public static string ToLatString(this SexagesimalCoordinate latitude, string format = "")
        {
            var latChar = latitude.DecimalValue >= 0 ? "N" : "S";
            var baseFormat = format.Length > 0
                ? format.Substring(0, 1)
                : "";
            var extFormat = format.Length > 1
                ? format.Substring(1)
                : "";
            switch (baseFormat)
            {
                case "d":
                    return $"{latitude.DecimalValue}°";
                case "g":
                    return string.IsNullOrEmpty(extFormat) 
                        ? $"{Math.Abs(latitude.Degrees)}° {latitude.Minutes}' {latitude.Seconds:F2}\" {latChar}"
                        : $"{Math.Abs(latitude.Degrees).ToString(extFormat)}° {latitude.Minutes.ToString(extFormat)}' {latitude.Seconds.ToString(extFormat + ".00")}\" {latChar}";
                case "m":
                    return $"{Math.Abs(latitude.Degrees)}° {latitude.Minutes + latitude.Seconds / 60.0:F4}' {latChar}";
            }

            return $"{latitude.DecimalValue}";
        }
        
        /// <summary>
        /// Format the given longitude
        /// Possible formats:
        /// d - Dezimalgrad, zum Beispiel -122.2°
        /// g - Grad, Minuten, Sekunden, zum Beispiel 122°05'06.24"W
        /// m - Grad, Dezimalminuten, zum Beispiel 122° 36.875' W
        ///     Grad, Dezimalgrad ohne Grad-Zeichen
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="format">Format to be used: d, g or m</param>
        /// <returns></returns>
        public static string ToLongString(this SexagesimalCoordinate longitude, string format = "")
        {
            var longChar = longitude.DecimalValue >= 0 ? "O" : "W";
            switch (format)
            {
                case "d":
                    return $"{longitude}°";
                case "g":
                    return $"{Math.Abs(longitude.Degrees)}° {longitude.Minutes}' {longitude.Seconds:F2}\" {longChar}";
                case "m":
                    return $"{Math.Abs(longitude.Degrees)}° {longitude.Minutes + longitude.Seconds / 60.0:F4}' {longChar}";
            }

            return $"{longitude}";
        }
    }
}