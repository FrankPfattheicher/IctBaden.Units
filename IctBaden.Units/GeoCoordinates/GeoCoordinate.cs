// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
namespace IctBaden.Units
{
    /*

    private string ConvertDecimalDegreesToSexagesimal(double decimalValueToConvert)
    {
        int degrees = (int)decimalValueToConvert;
        int minutes = (int)((decimalValueToConvert - degrees) * 60);
        int seconds = (int)((decimalValueToConvert - degrees - (minutes / 60)) * 3600);

        return String.Format("{0}° {1}' {2}''", degrees, minutes, seconds);
    }

    Bei der Darstellung der Koordinaten können Sie zwischen folgenden Formaten wählen:

    San Franzisko
    d - Dezimalgrad, zum Beispiel 37.7°, -122.2°
    g - Grad, Minuten, Sekunden, zum Beispiel 37°25'19.07"N, 122°05'06.24"W
    m - Grad, Dezimalminuten, zum Beispiel 32° 18.385' N 122° 36.875' W

    N, O -> positiv
    S, W -> negativ

    */
    
    
    /// <summary>
    /// Mostly compatible to System.Device version
    /// </summary>
    public class GeoCoordinate
    {
        /// <summary>
        /// [Grad] Breitengrad
        /// </summary>
        public double Latitude { get; private set; }
        /// <summary>
        /// [Grad] Längengrad 
        /// </summary>
        public double Longitude { get; private set; }

        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        
    }
}