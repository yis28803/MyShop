using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models;

namespace App.ViewModels;

public partial class OrderEditViewModel : ObservableObject
{

    private readonly IShopService _shopService;

    public ObservableCollection<Product> ProductList = new();
    public ObservableCollection<Customer> CustomerList = new();
    public ObservableCollection<OrderDetail> OrderDetailList = new();

    public OrderEditViewModel(IShopService repos)
    {
        _shopService = repos;
    }

    public async Task InitializeProperties(Order o)
    {
        await SyncProductsAndCustomer();

        if (o.CustomerId == null)
        {
            SelectedCustomer = CustomerList.Last(); // Unknown customer
        }
        else
        {
            SelectedCustomer = CustomerList.FirstOrDefault(c => c.Id == o.CustomerId);
        }

        EdittingOrder = o;

        var orderDetails = await _shopService.OrderService.GetOrderDetails(o.Id);

        OrderDetailList.Clear();
        foreach (var od in orderDetails)
        {
            OrderDetailList.Add(od);
        }
    }

    public async Task SyncProductsAndCustomer()
    {
        SyncProductList();
        var data2 = await _shopService.CustomerService.GetAsync();
        var nullCus = new Customer()
        {
            Id = -1,
            LastName = "Unknown"
        };
        CustomerList.Clear();
        foreach(var c in data2)
        {
            CustomerList.Add(c);
        }
        CustomerList.Add(nullCus);
    }

    public async void SyncProductList()
    {
        var data = await _shopService.ProductService.Search(SearchText);
        ProductList.Clear();
        foreach (var p in data)
        {
            ProductList.Add(p);
        }
    }

    [ObservableProperty]
    private string? searchText;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddOrderDetailCommand))]
    private int? count = 0;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveOrderCommand))]
    private Order? edittingOrder;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddOrderDetailCommand))]
    private Product? selectedProduct;
    
    [ObservableProperty] private Customer? selectedCustomer;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteOrderDetailCommand))]
    private OrderDetail? selectedOrderDetail;

    [RelayCommand(CanExecute = nameof(CanDeleteOrderDetail))]
    public void DeleteOrderDetail()
    {
        OrderDetailList.Remove(SelectedOrderDetail);

    }

    [RelayCommand(CanExecute = nameof(CanSaveOrder))]
    public void SaveOrder()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        EdittingOrder.OrderDetails.Clear();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        foreach (var od in OrderDetailList)
        {
            EdittingOrder.OrderDetails.Add(od);
        }
        EdittingOrder.CustomerId = SelectedCustomer?.Id;
        Debug.WriteLine(EdittingOrder.CustomerId);
        EdittingOrder.Customer = null;

        _ = _shopService.OrderService.UpsertOrder(EdittingOrder);
    }

    [RelayCommand(CanExecute = nameof(CanAddOrderDetail))]
    public void AddOrderDetail()
    {
        foreach (var od in OrderDetailList)
        {
#pragma warning disable CS8602 // CanExecute = nameof(CanAddOrderDetail)) checked.
            if (od.ProductId == SelectedProduct.Id)
            {
                var newOd = od.Clone(); //clone must remove product, if have time, create new method :)))
                newOd.Product = SelectedProduct;
                newOd.Quantity++;
                OrderDetailList.Add(newOd);
                OrderDetailList.Remove(od);
                Count = newOd.Quantity;
                return;
            }
#pragma warning restore CS8602 //CanExecute = nameof(CanAddOrderDetail)) checked
        }


#pragma warning disable CS8602 // CanExecute = nameof(CanAddOrderDetail)) checked.
        var detail = new OrderDetail()
        {
            ProductId = SelectedProduct.Id,
            Quantity = 1,
            OrderId = EdittingOrder.Id,
            Product = SelectedProduct,
            ImportPrice = SelectedProduct.ImportPrice,
            SalePrice = SelectedProduct.SalePrice
        };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        OrderDetailList.Add(detail);
        Count = detail.Quantity;
    }

    private bool CanDeleteOrderDetail() => SelectedOrderDetail != null ;
    private bool CanAddOrderDetail()
    {
        if(SelectedProduct == null)
        {
            return false;
        }  
        if(EdittingOrder == null) { return false; }
        
        foreach (var od in OrderDetailList)
        {
            if (od.ProductId == SelectedProduct.Id)
            {
                if(od.Quantity >= SelectedProduct.Quantity)
                {
                    return false;
                }
            }
        }
        if(SelectedProduct.Quantity <= 0)
        {
            return false;
        }
        return true;
    }
    private bool CanSaveOrder()
    {
        return EdittingOrder != null && EdittingOrder.CustomerId != null && EdittingOrder.OrderDetails.Count > 0;
    }

  
}
