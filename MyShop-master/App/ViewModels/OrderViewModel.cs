using System.Collections.ObjectModel;
using System.Diagnostics;
using App.Contracts.ViewModels;
using App.Dialog;
using App.Helpers;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Models;

namespace App.ViewModels;

public partial class OrderViewModel : ObservableRecipient, INavigationAware
{
    private readonly IShopService _shopService;

    public ObservableCollection<Order> OrderList { get; } = new();
    public ObservableCollection<OrderDetail> OrderDetailList { get; } = new();
    [NotifyCanExecuteChangedFor(nameof(OrderDeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(OrderEditCommand))]
    [ObservableProperty] 
    private Order? selectedOrder;

    //[ObservableProperty] private DateTime? startDate;
    //[ObservableProperty] private DateTime? endDate;
    //[ObservableProperty] private int pageSize = 10;
    

    //TODO: Use DateConverter and remove these 2 property
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    public int PageSize { get; set; } = 10;


    public OrderViewModel(IShopService repos)
    {
        _shopService = repos;
    }

    [NotifyCanExecuteChangedFor(nameof(PreviousPageClickCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextPageClickCommand))]
    [ObservableProperty]
    private int? selectedPage = 1;
    [ObservableProperty] private int totalPageCount = 1;
    [ObservableProperty] private int totalOrderCount = 0;

    public ObservableCollection<int> OrderPageNumberList { get; } = new();
 

    public  void OnNavigatedTo(object parameter)
    {
    }


    public async Task SyncOrders()
    {
        SelectedOrder = null;
        var data = await _shopService.OrderService.QueryOrderPage(StartDate, EndDate, PageSize, SelectedPage);
        OrderList.Clear();
        foreach (var o in data)
        {
            OrderList.Add(o);
        }
    }

    public async Task SyncOrderDetailList()
    {
        if(SelectedOrder == null)
        {
            return;
        }
        var data = await _shopService.OrderService.GetOrderDetails(SelectedOrder.Id);
        OrderDetailList.Clear();
        foreach (var o in data)
        {
            OrderDetailList.Add(o);
        }
    }

    public async Task GetPageListAsync()
    {
        await GetTotalPage();
        SelectedPage = null;
        OrderPageNumberList.Clear();
        for (var i = 1; i <= TotalPageCount; i++)
        {
            OrderPageNumberList.Add(i);
        }
        SelectedPage = 1;
    }
    public async Task GetTotalPage()
    {
        TotalOrderCount = await _shopService.OrderService.GetTotalOrderCount(StartDate, EndDate);
        TotalPageCount = (TotalOrderCount - 1) / PageSize + 1;
    }

    [RelayCommand(CanExecute = nameof(CanDescreasePage))]
    private void PreviousPageClick()
    {
        SelectedPage--;
    }

    [RelayCommand(CanExecute = nameof(CanIncreasePage))]
    private void NextPageClick()
    {
        SelectedPage++;
    }

    [RelayCommand(CanExecute = nameof(CanDeleteOrder))]
    private async void OrderDelete()
    {
        await _shopService.OrderService.DeleteAsync(SelectedOrder!.Id);
        await GetPageListAsync();
    }

    [RelayCommand(CanExecute = nameof(CanEditOrder))]
    private void OrderEdit(AppBarButton invoker)
    {
    }
    private bool CanDeleteOrder()
    {
        return SelectedOrder != null;
    }

    private bool CanEditOrder()
    {
        return SelectedOrder != null;
    }
    private bool CanDescreasePage()
    {
        return SelectedPage > 1 && SelectedPage != null;
    }
    private bool CanIncreasePage()
    {
        return SelectedPage < TotalPageCount && SelectedPage != null;
    }

    public void OnNavigatedFrom()
    {
    }

}
