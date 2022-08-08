namespace Uroskur.Utils;

public static class GpxParser
{
    private const string DefaultNamespace = "http://www.topografix.com/GPX/1/1";

    public static IEnumerable<Location> GpxToLocations(string xml)
    {
        using var sourceStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
        var xmlReader = XmlReader.Create(sourceStream, new XmlReaderSettings
        {
            Async = true
        });

        var xmlSerializer = new XmlSerializer(typeof(Gpx), DefaultNamespace);
        if (xmlSerializer.Deserialize(xmlReader) is not Gpx gpx)
        {
            return Array.Empty<Location>();
        }

        var trksegTrkpt = gpx.Trk?.Trkseg?.TrksegTrkpt;

        return trksegTrkpt == null
            ? Array.Empty<Location>()
            : trksegTrkpt.Select(trkpt => new Location(trkpt.Lat, trkpt.Lon)).ToImmutableArray();
    }
}