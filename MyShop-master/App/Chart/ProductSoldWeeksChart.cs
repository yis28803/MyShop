using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Models;

namespace App.Chart;

public class ProductSoldWeeksChart : ChartBase
{
    public ProductSoldWeeksChart(string title) : base(title){ }

    public void SyncProductSoldWeeksChart(IEnumerable<ProductSoldCountWeek> values)
    {
        SyncProductSoldWeeksSeries(values);
        SetXAxis(values);
        SetYAxis();
    }

    private void SetYAxis()
    {
        var axis = new Axis
        {
            //axis.Labels = "Inome"; //format string
            Name = "Sold Quantity",
        };
        YAxes.Add(axis);
    }

    private void SetXAxis(IEnumerable<ProductSoldCountWeek> values)
    {
        if (XAxes.Count > 0)
        {
            return;
        }

        var x = new DateTimeAxis(TimeSpan.FromDays(7), date => date.ToString("dd/MM/yyy"))
        {
            Name = "Start of Week"
        };

        XAxes.Add(x);
    }
    private void SyncProductSoldWeeksSeries(IEnumerable<ProductSoldCountWeek> values)
    {
        var data = new ObservableCollection<DateTimePoint>();
        foreach (var item in values)
        {
            data.Add(new DateTimePoint(item.StartOfWeek, item.TotalQuantitySold));
        }
        ISeries mainSeries = new LineSeries<DateTimePoint>()
        {
            Name = "Quantity",
            LineSmoothness = 0,
            Values = data,
            GeometrySize = 3
        };
        
        Series.Clear();
        Series.Add(mainSeries);
    }

}