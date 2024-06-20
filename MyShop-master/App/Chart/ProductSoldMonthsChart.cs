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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Chart;

public class ProductSoldMonthsChart : ChartBase
{
    public ProductSoldMonthsChart(string title) : base(title){ }

    public void SyncProductSoldMonthsChart(IEnumerable<ProductSoldCountMonth> values)
    {
        SyncProductSoldMonthsSeries(values);
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

    private void SetXAxis(IEnumerable<ProductSoldCountMonth> values)
    {
        if (XAxes.Count > 0)
        {
            return;
        }
        var listMonth = new List<string>();
        foreach (var item in values)
        {
            listMonth.Add($"{item.Month}/{item.Year}");
        }

        var x = new Axis
        {
            Labels = listMonth.ToArray(),
            Name = "Month/Year",
        };
        XAxes.Add(x);
    }
    public void SyncProductSoldMonthsSeries(IEnumerable<ProductSoldCountMonth> values)
    {
        var data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.TotalQuantitySold);
        }
        var color = new[]
        {
            new SKColor(137,255,253),
            new SKColor(255,255,255)
        };
        ISeries sr = new LineSeries<decimal>()
        {
            Name = "Quantity",
            LineSmoothness = 0,
            Values = data,
            Stroke = new LinearGradientPaint(color) { StrokeThickness = 2 },
            GeometrySize = 3,
            Fill = new LinearGradientPaint(color, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
        };

        Series.Clear();
        Series.Add(sr);
    }
}
