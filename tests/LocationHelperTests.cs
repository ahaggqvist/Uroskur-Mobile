namespace Uroskur.Tests;

public class LocationHelperTests
{
    [Fact]
    public void Total_distance_is_equal_to_expected()
    {
        var locations = GpxParser.GpxToLocations(Gxp.FileAsString());
        var totalDistance = LocationHelper.CalculateTotalDistance(locations);
        Assert.Equal(45, Math.Round(totalDistance));
    }

    [Fact]
    public void Number_of_locations_is_equal_to_expected()
    {
        var gpxToLocations = GpxParser.GpxToLocations(Gxp.FileAsString());
        var locations = LocationHelper.FilterOutLocationsAtEvenDistances(gpxToLocations)?.ToList();
        Assert.Equal(4, locations?.Count);
    }
}