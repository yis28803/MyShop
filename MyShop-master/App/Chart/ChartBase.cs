using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace App.Chart;
public partial class ChartBase : ObservableObject
{
    //TODO: fix a bug chart do not show Title , despite of using observableproperty
    public LabelVisual Title = new()
    {
        Rotation = 0,
        TextSize = 25,
        Padding = new LiveChartsCore.Drawing.Padding(8),
        Paint = new SolidColorPaint(SKColors.Transparent)
    };
    public ObservableCollection<ISeries> Series { get; set; } = new();
    public ObservableCollection<ICartesianAxis> XAxes { get; set; } = new();
    public ObservableCollection<ICartesianAxis> YAxes { get; set; } = new();
    public void SetDefaultTitle(string value)
    {
        Title.Text = value;
    }

    public ChartBase(string title)
    {
        SetDefaultTitle(title);
    }
}
