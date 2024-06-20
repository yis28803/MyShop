using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Microsoft.UI.Xaml.Media;
using LiveChartsCore.Kernel.Drawing;
using Models;
using SkiaSharp;
using System.Diagnostics;
using LiveChartsCore.Kernel.Sketches;

namespace App.Chart;

//TODO: separate this class to 4 class
public partial class IncomeLiveChartPack : ChartBase
{
    public IncomeLiveChartPack(string title) : base(title)
    {
    }
    //public ObservableCollection<ISeries> Series { get; set; } = new ();


    //public ObservableCollection<ICartesianAxis> XAxes { get; set; } = new();
    //public ObservableCollection<ICartesianAxis> YAxes { get; set; } = new();
    public SolidColorPaint DefaultLegendPaintText { get; set; } =
        new SolidColorPaint
        {
            Color = SKColors.Transparent,
            SKTypeface = SKTypeface.FromFamilyName("Courier New")
        };

    public void SyncDayIncomeChart(IEnumerable<DayIncome> values)
    {
        SetDefaultTitle("Days Income");
        SyncDayInComeSeries(values);
        SetDayIncomeXAxis();
        SetYIncomAxis();
    }
    public void SyncWeekIncomeChart(IEnumerable<WeekIncome> values)
    {
        SetDefaultTitle("Weeks Income");
        SetWeekIncomeXAxis(values);
        SyncWeekInComeSeries(values);
        SetYIncomAxis();
    }
    public void SyncMonthIncomeChart(IEnumerable<MonthIncome> values)
    {
        SetDefaultTitle("Months Income");
        SyncMonthInComeSeries(values);
        SetMonthIncomeXAxis(values);
        SetYIncomAxis();
    }
    public void SyncYearIncomeChart(IEnumerable<YearIncome> values)
    {
        SetDefaultTitle("Years Income");
        SyncYearInComeSeries(values);
        SetYearIncomeXAxis(values);
        SetYIncomAxis();
    }
      
    //used for day income and week income
    public void SetDayIncomeXAxis()
    {
        if(XAxes.Count > 0)
        {
            return;
        }

        var x = new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yyy"))
        {
            Name = "Date"
        };

        XAxes.Add(x);
    }
    public void SetWeekIncomeXAxis(IEnumerable<WeekIncome> data)
    {
        if(XAxes.Count > 0)
        {
            return;
        }
        var listWeek = new List<string>();
        foreach (var item in data)
        {
            listWeek.Add(item.StartOfWeek.ToString("dd/MM/yyyy"));
        }

        var x = new Axis
        {
            //Labels = data.Select(x => x.StartOfWeek.ToString("dd/mm/yyyy")).ToArray()
            Labels = listWeek.ToArray(),
            Name = "Start Of Week (Monday)",
        };
        XAxes.Add(x);
    }
    public void SetMonthIncomeXAxis(IEnumerable<MonthIncome> data)
    {
        if (XAxes.Count > 0)
        {
            return;
        }
        var listMonth = new List<string>();
        foreach (var item in data)
        {
            listMonth.Add($"{item.Month}/{item.Year}");
        }

        var x = new Axis
        {
            //Labels = data.Select(x => x.StartOfWeek.ToString("dd/mm/yyyy")).ToArray()
            Labels = listMonth.ToArray(),
            Name = "Month/Year",
        };
        XAxes.Add(x);
    }
    public void SetYearIncomeXAxis(IEnumerable<YearIncome> data)
    {
        if (XAxes.Count > 0)
        {
            return;
        }
        var listYear = new List<string>();
        foreach (var item in data)
        {
            listYear.Add(item.Year.ToString());
        }

        var x = new Axis
        {
            Labels = listYear.ToArray(),
            Name = "Year",
        };
        XAxes.Add(x);
    }

    //set 2 series: revenue and profit  
    public void SyncDayInComeSeries(IEnumerable<DayIncome> values)
    {

        var data = new ObservableCollection<DateTimePoint>();
        foreach (var item in values)
        {
            data.Add(new DateTimePoint(item.OrderPlaced.Date, (double)item.Revenue));
        }

        ISeries revenueSeries = new ColumnSeries<DateTimePoint>()
        {
            Name = "Revenue",
            Stroke = null,
            Values = data
            
        };

        data = new ObservableCollection<DateTimePoint>();
        foreach (var item in values)
        {
            data.Add(new DateTimePoint(item.OrderPlaced.Date, (double)item.Profit));
        }

        ISeries profitSeries = new LineSeries<DateTimePoint>()
        {
            Name = "Profit",
            LineSmoothness = 0,
            Values = data,
            GeometrySize = 0
        };
        Series.Clear();
        Series.Add(revenueSeries);
        Series.Add(profitSeries);
    }
    public void SyncWeekInComeSeries(IEnumerable<WeekIncome> values)
    {
        //var data = new ObservableCollection<DateTimePoint>();
        //foreach (var item in values)
        //{
        //    data.Add(new DateTimePoint(item.StartOfWeek.Date, (double)item.Revenue));
        //}
        //ISeries revenueSeries = new ColumnSeries<DateTimePoint>()
        //{
        //    Name = "Revenue",
        //    Stroke = null,
        //    Values = data
            
        //};

        var data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.Revenue);
        }
        ISeries revenueSeries = new ColumnSeries<decimal>()
        {
            Name = "Revenue",
            Stroke = null,
            Values = data
            
        };


        //data = new ObservableCollection<DateTimePoint>();
        //foreach (var item in values)
        //{
        //    data.Add(new DateTimePoint(item.StartOfWeek.Date, (double)item.Profit));
        //}

        //ISeries profitSeries = new LineSeries<DateTimePoint>()
        //{
        //    Name = "Profit",
        //    LineSmoothness = 0,
        //    Values = data,
        //    GeometrySize = 0
        //};
        data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.Profit);
        }

        ISeries profitSeries = new LineSeries<decimal>()
        {
            Name = "Profit",
            LineSmoothness = 0,
            Values = data,
            GeometrySize = 0
        };
            

        Series.Clear();
        Series.Add(revenueSeries);
        Series.Add(profitSeries);
    }   
    public void SyncMonthInComeSeries(IEnumerable<MonthIncome> values)
    {
        var data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.Revenue);
        }
        ISeries revenueSeries = new LineSeries<decimal>()
        {
            Name = "Revenue",
            LineSmoothness = 0,
            Values = data,
            GeometrySize = 3,
        };

        data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.Profit);
        }

        ISeries profitSeries = new LineSeries<decimal>()
        {
            Name = "Profit",
            LineSmoothness = 0,
            Values = data,
            GeometrySize = 3
        };

        Series.Clear();
        Series.Add(revenueSeries);
        Series.Add(profitSeries);
    }
    public void SyncYearInComeSeries(IEnumerable<YearIncome> values)
    {
        var data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.Revenue);
        }
        ISeries revenueSeries = new ColumnSeries<decimal>()
        {
            Name = "Revenue",
            Stroke = null,
            Values = data,
            IgnoresBarPosition = true
        };

        data = new ObservableCollection<decimal>();
        foreach (var item in values)
        {
            data.Add(item.Profit);
        }

        ISeries profitSeries = new ColumnSeries<decimal>()
        {
            Name = "Profit",
            Stroke = null,
            Values = data,
            MaxBarWidth = 30,
            IgnoresBarPosition = true
        };

        Series.Clear();
        Series.Add(revenueSeries);
        Series.Add(profitSeries);
    }

   
    private void SetYIncomAxis()
    {
        var axis = new Axis
        {
            //axis.Labels = "Inome"; //format string
            Name = "Income (Dollars)",
            MinLimit = 0,
        };
        YAxes.Add(axis);
    }
}
