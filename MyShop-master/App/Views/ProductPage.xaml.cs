using System.Diagnostics;
using App.ViewModels;

using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
namespace App.Views;

public sealed partial class ProductPage : Page
{
    public ProductViewModel ViewModel
    {
        get;
    }

    public ProductPage()
    {
        ViewModel = App.GetService<ProductViewModel>();
        InitializeComponent();
    }

    private  void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(ViewModel.SelectedCategory != null)
        {
            Debug.WriteLine("category Changed");
            _ = ViewModel.GetPageListAsync();

        }
    }

    private  void OnSortSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Debug.WriteLine("sortType Changed");
        if(ViewModel.SelectedPage == 1)
        {
            _= ViewModel.SyncProducts();
        }
        else
        {
            ViewModel.SelectedPage = 1;
        }
    }

    private  void OnSearchNameChanged(object sender, TextChangedEventArgs e)
    {

        Debug.WriteLine("SearchName Changed");
        _ = ViewModel.GetPageListAsync();

    }

    private  void OnMinSalePriceChanged(object sender, NumberBoxValueChangedEventArgs e)
    {

        Debug.WriteLine("MinpriceChanged Changed");
        _= ViewModel.GetPageListAsync();

    }

    private void OnMaxSalePriceChanged(object sender, NumberBoxValueChangedEventArgs e)
    {

        Debug.WriteLine("MaxpriceChanged Changed");
        _ = ViewModel.GetPageListAsync();

    }

    private  void OnPageSizeChanged(object sender, NumberBoxValueChangedEventArgs e)
    {

        Debug.WriteLine("Pagesize Changed");
        _ = ViewModel.GetPageListAsync();

    }

    private void OnPageChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel.SelectedPage != null)
        {
            Debug.WriteLine("Page Changed");
            _ = ViewModel.SyncProducts();
        }
    }

    private void OnShowStatisticsClick(object sender, RoutedEventArgs e)
    {
        var dialog = new ProductSoldChartDialog(ViewModel.SelectedProduct.Id)
        {
            Title = "Product Sold Statistics",
            XamlRoot = XamlRoot
        };
        
        _ = dialog.ShowAsync();
    }
}
