using System.Collections.ObjectModel;
using System.Diagnostics;
using App.ViewModels;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Models;

namespace App.Views;

public sealed partial class OrderPage : Page
{
    public OrderViewModel ViewModel
    {
        get;
    }

    public OrderEditDialog EditDialog {
        get; set;
    } = null!;

    public Collection<Product> ProductListParameter { get; set; } = null!;
    public OrderPage()
    {
        ViewModel = App.GetService<OrderViewModel>();
        InitializeComponent();
    }

   

    private void OnPageSizeChanged(object _, NumberBoxValueChangedEventArgs e)
    {
        _= ViewModel.GetPageListAsync();
    }

    private void OnPageChanged(object _, SelectionChangedEventArgs e)
    {
        if(ViewModel.SelectedPage != null)
        {
            _ = ViewModel.SyncOrders();
        }
    }

    private  void OnStartDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    {
        ViewModel.StartDate = sender.Date; //fix a bug that source update later than target, dont know why
        _= ViewModel.GetPageListAsync();
    }
    private void OnEndDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
    { 
        ViewModel.EndDate = sender.Date; //fix a bug that source update later than target
        _ = ViewModel.GetPageListAsync();
    }

    private void OnOrderSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        

        if (ViewModel.SelectedOrder != null)
        {
            _ = ViewModel.SyncOrderDetailList();
        }
    }

    private async void OnAddOrderClicked(object sender, RoutedEventArgs e)
    {
        var o = new Order() { CustomerId = null, OrderPlaced = DateTime.Now, OrderFulfilled = null, OrderDetails = new List<OrderDetail>() };
        EditDialog = new OrderEditDialog(o)
        {
            Title = "Add Order",
            XamlRoot = XamlRoot
        };
        await EditDialog.ShowAsync();
        await ViewModel.GetPageListAsync();
    }
    private async void OnEditOrderClicked(object sender, RoutedEventArgs e)
    {

        EditDialog = new OrderEditDialog((Order)OrderListDataGrid.SelectedItem)
        {
            Title = "Edit Order",   
            XamlRoot = XamlRoot
        };

        await EditDialog.ShowAsync();
        _ = ViewModel.SyncOrders();
    }

}
