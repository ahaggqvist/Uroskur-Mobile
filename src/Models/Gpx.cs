﻿namespace Uroskur.Models;

[XmlRoot(ElementName = "trkpt", Namespace = "http://www.topografix.com/GPX/1/1")]
public class Trkpt
{
    [XmlAttribute("lat")] public double Lat { get; init; }
    [XmlAttribute("lon")] public double Lon { get; init; }
}

[XmlRoot(ElementName = "trkseg", Namespace = "http://www.topografix.com/GPX/1/1")]
public class Trkseg
{
    [XmlElement("trkpt")] public List<Trkpt>? TrksegTrkpt { get; init; }
}

[XmlRoot(ElementName = "trk", Namespace = "http://www.topografix.com/GPX/1/1")]
public class Trk
{
    [XmlElement("trkseg")] public Trkseg? Trkseg { get; init; }
}

[XmlRoot(ElementName = "gpx", Namespace = "http://www.topografix.com/GPX/1/1")]
public class Gpx
{
    [XmlElement("trk")] public Trk? Trk { get; init; }
}