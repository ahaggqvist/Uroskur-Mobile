namespace Uroskur.Models;

public class HourlyForecast
{
    public DateTime Dt { get; set; }
    public long UnixTimestamp { get; set; }
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public double Uvi { get; set; }
    public double Cloudiness { get; set; }
    public double WindSpeed { get; set; }
    public double WindGust { get; set; }
    public long WindDeg { get; set; }
    public double Pop { get; set; }
    public long IconId { get; set; }
}