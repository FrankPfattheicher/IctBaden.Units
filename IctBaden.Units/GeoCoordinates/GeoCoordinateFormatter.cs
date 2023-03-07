// ReSharper disable CommentTypo

using System.Globalization;

namespace IctBaden.Units;

public static class GeoCoordinateFormatter
{
    /// <summary>
    /// Format the given geo-coordinate
    /// Possible formats:
    /// d - Dezimalgrad, zum Beispiel 37.7°, -122.2°
    /// g - Grad, Minuten, Sekunden, zum Beispiel 37°25'19.07"N, 122°05'06.24"W
    /// m - Grad, Dezimalminuten, zum Beispiel 32° 18.385' N 122° 36.875' W
    ///     Grad, Dezimalgrad ohne Grad-Zeichen
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="format">Format to be used: d, g or m</param>
    /// <returns></returns>
    public static string ToString(this GeoCoordinate coordinate, CultureInfo cultureInfo, string format = "")
    {
        var latitude = new SexagesimalCoordinate(coordinate.Latitude);
        var longitude = new SexagesimalCoordinate(coordinate.Longitude);
        return $"{latitude.ToLatString(cultureInfo, format)}, {longitude.ToLongString(cultureInfo, format)}";
    }
    
    public static string ToString(this GeoCoordinate coordinate, string format = "")
    {
        var cultureInfo = CultureInfo.CurrentUICulture; 
        var latitude = new SexagesimalCoordinate(coordinate.Latitude);
        var longitude = new SexagesimalCoordinate(coordinate.Longitude);
        return $"{latitude.ToLatString(cultureInfo, format)}, {longitude.ToLongString(cultureInfo, format)}";
    }

}