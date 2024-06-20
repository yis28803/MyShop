using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Models;
using SkiaSharp;

namespace App.Chart;
public class ProductSoldDaysChart : ChartBase
{
    public ProductSoldDaysChart(string title) : base(title){ }

    public void SyncProductSoldDaysChart(IEnumerable<ProductSoldCountDay> values)
    {
        SyncProductSoldDaysSeries(values);
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

    private void SetXAxis(IEnumerable<ProductSoldCountDay> values)
    {
        if (XAxes.Count > 0)
        {
            return;
        }

        var x = new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yyy"))
        {
            Name = "Date"
        };

        XAxes.Add(x);
    }
    private void SyncProductSoldDaysSeries(IEnumerable<ProductSoldCountDay> values)
    {
        
        var data = new ObservableCollection<DateTimePoint>();
        foreach (var item in values)
        {
            data.Add(new DateTimePoint(item.Date, item.TotalQuantitySold));
        }

        

        ISeries mainSeries = new ColumnSeries<DateTimePoint>()
        {
            Name = "Quantity",
            Values = data,
            Stroke = null,
        };

        Series.Clear();
        Series.Add(mainSeries);
    }
}
