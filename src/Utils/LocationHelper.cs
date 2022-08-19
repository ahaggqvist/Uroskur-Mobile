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

    public static double CalculateTotalDistance(IEnumerable<Location> locations)
    {
        var total = 0d;
        foreach (var (_, currentLocation, nextLocation) in locations.GetItems())
        {
            total += CalculateDistanceBetweenLocations(currentLocation.Lat, currentLocation.Lon,
                nextLocation.Lat,
                nextLocation.Lon);
        }

        return total;
    }

    public static IEnumerable<Location> FilterOutLocationsAtEvenDistances(IEnumerable<Location> locations)
    {
        var locationsAtEvenDistances = new List<Location>();
        var current = 0;
        var total = 0d;
        var locationsArray = locations as Location[] ?? locations.ToArray();

        foreach (var (_, currentLocation, nextLocation) in locationsArray.GetItems())
        {
            total += CalculateDistanceBetweenLocations(currentLocation.Lat, currentLocation.Lon,
                nextLocation.Lat,
                nextLocation.Lon);

            var totalRounded = Math.Round(total, 1);
            if (totalRounded % EvenDistance != 0 || Math.Abs(totalRounded - current) == 0)
            {
                continue;
            }

            locationsAtEvenDistances.Add(currentLocation);
            current = (int)totalRounded;
        }

        if (locationsAtEvenDistances.Count == 0 && locationsArray.Any())
        {
            locationsAtEvenDistances.Add(locationsArray[0]);
        }

        return locationsAtEvenDistances;
    }
}