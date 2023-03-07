using System;
using System.Globalization;

// ReSharper disable CommentTypo

namespace IctBaden.Units;

public static class SexagesimalCoordinateFormatter
{
    public static string ToString(this SexagesimalCoordinate coordinate, CultureInfo cultureInfo, string format = "")
    {
        return coordinate.Type == SexagesimalCoordinate.CoordinateType.Latitude
            ? ToLatString(coordinate, cultureInfo, format)
            : ToLongString(coordinate, cultureInfo, format);
    }

    public static string ToString(this SexagesimalCoordinate longitude, string format = "")
    {
        return ToString(longitude, CultureInfo.CurrentUICulture, format);
    }


    /// <summary>
    /// Format the given latitude
    /// Possible formats:
    /// d - Dezimalgrad, zum Beispiel 37.7°
    /// g - Grad, Minuten, Sekunden, zum Beispiel 37°25'19.07"N
    /// m - Grad, Dezimalminuten, zum Beispiel 32° 18.385' N
    ///     Grad, Dezimalgrad ohne Grad-Zeichen
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="cultureInfo"></param>
    /// <param name="format">Format to be used: d, g or m</param>
    /// <returns></returns>
    public static string ToLatString(this SexagesimalCoordinate latitude, CultureInfo cultureInfo, string format = "")
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
                return $"{latitude.DecimalValue.ToString(cultureInfo)}°";
            case "g":
                return string.IsNullOrEmpty(extFormat) 
                    ? $"{Math.Abs(latitude.Degrees).ToString(cultureInfo)}° {latitude.Minutes.ToString(cultureInfo)}' {latitude.Seconds.ToString("F2", cultureInfo)}\" {latChar}"
                    : $"{Math.Abs(latitude.Degrees).ToString(extFormat, cultureInfo)}° {latitude.Minutes.ToString(extFormat, cultureInfo)}' {latitude.Seconds.ToString(extFormat + ".00", cultureInfo)}\" {latChar}";
            case "m":
                return $"{Math.Abs(latitude.Degrees).ToString(cultureInfo)}° {(latitude.Minutes + latitude.Seconds / 60.0).ToString("F4", cultureInfo)}' {latChar}";
        }

        return $"{latitude.DecimalValue.ToString(cultureInfo)}";
    }

    public static string ToLatString(this SexagesimalCoordinate longitude, string format = "")
    {
        return ToLatString(longitude, CultureInfo.CurrentUICulture, format);
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
    /// <param name="cultureInfo"></param>
    /// <param name="format">Format to be used: d, g or m</param>
    /// <returns></returns>
    public static string ToLongString(this SexagesimalCoordinate longitude, CultureInfo cultureInfo, string format = "")
    {
        var longChar = longitude.DecimalValue >= 0 ? "O" : "W";
        var baseFormat = format.Length > 0
            ? format.Substring(0, 1)
            : "";
        var extFormat = format.Length > 1
            ? format.Substring(1)
            : "";
        switch (baseFormat)
        {
            case "d":
                return $"{longitude.DecimalValue.ToString(cultureInfo)}°";
            case "g":
                return string.IsNullOrEmpty(extFormat) 
                    ? $"{Math.Abs(longitude.Degrees).ToString(cultureInfo)}° {longitude.Minutes.ToString(cultureInfo)}' {longitude.Seconds.ToString("F2", cultureInfo)}\" {longChar}"
                    : $"{Math.Abs(longitude.Degrees).ToString(extFormat, cultureInfo)}° {longitude.Minutes.ToString(extFormat, cultureInfo)}' {longitude.Seconds.ToString(extFormat + ".00", cultureInfo)}\" {longChar}";
            case "m":
                return $"{Math.Abs(longitude.Degrees).ToString(cultureInfo)}° {(longitude.Minutes + longitude.Seconds / 60.0).ToString("F4", cultureInfo)}' {longChar}";
        }

        return $"{longitude.DecimalValue.ToString(cultureInfo)}";
    }

    public static string ToLongString(this SexagesimalCoordinate longitude, string format = "")
    {
        return ToLongString(longitude, CultureInfo.CurrentUICulture, format);
    }
    
}