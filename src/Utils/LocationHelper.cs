namespace Uroskur.Utils;

public static class LocationHelper
{
    private const int EvenDistance = 10;

    private static double CalculateDistanceBetweenLocations(double lat1, double lon1, double lat2,
        double lon2)
    {
        double Rad(double angle)
        {
            return angle * 0.017453292519943295769236907684886127d;
        }

        double Havf(double diff)
        {
            return Math.Pow(Math.Sin(Rad(diff) / 2d), 2);
        }

        return 12745.6 *
               Math.Asin(Math.Sqrt(
                   Havf(lat2 - lat1) +
                   Math.Cos(Rad(lat1)) * Math.Cos(Rad(lat2)) *
                   Havf(lon2 - lon1)));
    }

    public static IEnumerable<Location> FilterOutLocationsAtEvenDistances(IEnumerable<Location> locations)
    {
        var distances = new List<Location>();
        var current = 0;
        var total = 0D;
        var locationsArray = locations as Location[] ?? locations.ToArray();
        foreach (var (_, _, currentLocation, nextLocation) in locationsArray.GetItems())
        {
            total += CalculateDistanceBetweenLocations(currentLocation.Lat, currentLocation.Lon,
                nextLocation.Lat,
                nextLocation.Lon);

            var d = Math.Round(total, 1);
            if (d % EvenDistance != 0 || Math.Abs(d - current) == 0)
            {
                continue;
            }

            distances.Add(currentLocation);
            current = (int)d;
        }

        // Add one location if route length is less than minimum even distance
        if (distances.Count == 0 && locationsArray.Any())
        {
            distances.Add(locationsArray[0]);
        }

        return distances;
    }
}