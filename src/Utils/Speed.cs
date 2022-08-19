namespace Uroskur.Utils;

public class Speed : Enumeration
{
    public static readonly Speed Ten = new(1, "10");
    public static readonly Speed Twenty = new(2, "20");
    public static readonly Speed Thirty = new(3, "30");
    public static readonly Speed Forty = new(4, "40");
    public static readonly Speed Fifty = new(5, "50");

    public Speed() { }

    public Speed(int id, string name)
        : base(id, name) { }
}