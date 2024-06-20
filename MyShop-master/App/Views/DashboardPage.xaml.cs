using App.ViewModels;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Devices.Input;

namespace App.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();
    }

    private void OnChartPointerEnter(object sender, PointerRoutedEventArgs e)
    {
        //disable scrollviewer scroll
        PageScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
    }
    private void OnChartPointerExit(object sender, PointerRoutedEventArgs e)
    {
        //enable scrollviewer scroll
        PageScrollViewer.VerticalScrollMode = ScrollMode.Enabled;
    }
}
