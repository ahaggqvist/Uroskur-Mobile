namespace Uroskur.Utils;

public class Day : Enumeration
{
    public static readonly Day Today = new(1, "Today");
    public static readonly Day Tomorrow = new(2, "Tomorrow");

    public Day() { }

    public Day(int id, string name)
        : base(id, name) { }
}