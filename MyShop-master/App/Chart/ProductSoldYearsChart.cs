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

public class ProductSoldYearsChart : ChartBase
{
    public ProductSoldYearsChart(string title) : base(title){ }

    public void SyncProductSoldYearsChart(IEnumerable<ProductSoldCountYear> values)
    {
        SyncProductSoldYearsSeries(values);
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

    private void SetXAxis(IEnumerable<ProductSoldCountYear> values)
    {
        if (XAxes.Count > 0)
        {
            return;
        }
        var listYear = new List<string>();
        foreach (var item in values)
        {
            listYear.Add($"{item.Year}");
        }

        var x = new Axis
        {
            Labels = listYear.ToArray(),
            Name = "Year",
        };
        XAxes.Add(x);
    }

    private void SyncProductSoldYearsSeries(IEnumerable<ProductSoldCountYear> values)
    {
        var data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.TotalQuantitySold);
        }
        ISeries mainSeries = new ColumnSeries<decimal>()
        {
            Name = "Quantity",
            Stroke = null,
            Values = data,
            Fill = new SolidColorPaint(SKColors.DarkCyan)
        };

        Series.Clear();
        Series.Add(mainSeries);
    }
}
