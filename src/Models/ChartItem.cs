using Microcharts;

namespace Uroskur.Models;

public class ChartItem
{
    public ChartItem(string name, Chart chart)
    {
        Name = name;
        Chart = chart;
    }

    public string Name { get; }
    public Chart Chart { get; }
}