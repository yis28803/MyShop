using App.Contracts.ViewModels;
using BusinessLogic.Interfaces;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using App.Helpers;
using App.Dialog;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;

namespace App.ViewModels;

public partial class ProductViewModel : ObservableRecipient, INavigationAware
{
    private readonly IShopService _shopService;

    [NotifyCanExecuteChangedFor(nameof(CategoryDeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(CategoryEditCommand))]
    [ObservableProperty] 
    private Category? selectedCategory;

    [ObservableProperty] private string? selectedSortOrderType;
    [ObservableProperty] private string? searchName;
    [ObservableProperty] private string? minSalePrice;
    [ObservableProperty] private string? maxSalePrice;
    [ObservableProperty] private int pageSize = 10;

    [NotifyCanExecuteChangedFor(nameof(PreviousPageClickCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextPageClickCommand))]
    [ObservableProperty]
    private int? selectedPage = 1;

    [ObservableProperty] private int totalPageCount = 1;
    [ObservableProperty] private int totalProductCount = 0;

    [NotifyCanExecuteChangedFor(nameof(ProductDeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(ProductEditCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowStatisticsCommand))]
    [ObservableProperty] 
    private Product? selectedProduct;
    public ObservableCollection<Category> CategoryList { get; } = new();
    public ObservableCollection<Product> ProductList { get; } = new();
    public ObservableCollection<int> ProductPageNumberList { get; } = new();
    public List<string> SortOrderTypeList { get; } = new() { "Id Asc", "Id Desc", "Name Asc", "Name Desc", "Sale Price Asc", "Sale Price Desc", "Quantity Asc", "Quantity Desc" };
  
    public ProductViewModel(IShopService shop)
    {
        _shopService = shop;
        
    }


    public async void OnNavigatedTo(object parameter)
    {
        await SyncCategoris();
    }

    public async Task SyncCategoris()
    {
        SelectedCategory = null;
        CategoryList.Clear();
        var data = await _shopService.ProductService.GetCategories();
        foreach (var category in data)
        {
            CategoryList.Add(category);
        }
    }
    
    public async Task SyncProducts()
    {
        SelectedProduct = null;
        ProductList.Clear();
        var data = await _shopService.ProductService.QueryProductPage(SelectedCategory, SearchName, MinSalePrice.ParseDecimal(), MaxSalePrice.ParseDecimal(), SelectedSortOrderType, PageSize, SelectedPage);
        foreach (var product in data)
        {
            ProductList.Add(product);
        }
    }
    
    public async Task GetPageListAsync ()
    {
        await GetTotalPage();
        SelectedPage = null;
        ProductPageNumberList.Clear();
        for (var i = 1; i <= TotalPageCount; i++)
        {
            ProductPageNumberList.Add(i);
        }
        SelectedPage = 1;
    }
    public async Task GetTotalPage()
    {
        TotalProductCount = await _shopService.ProductService.GetTotalProductCountAsync(SelectedCategory, SearchName, MinSalePrice.ParseDecimal(), MaxSalePrice.ParseDecimal());
        TotalPageCount = (TotalProductCount - 1) / PageSize + 1;
    }

    [RelayCommand(CanExecute = nameof(CanDeleteProduct))]
    private void ShowStatistics()
    {
        
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

    [RelayCommand]
    private async void ProductAdd(AppBarButton invoker)
    {
        var product = new Product()
        {
            Name = "New Product",
            SalePrice = 0,
            ImportPrice = 0,
            Quantity = 0,
            CategoryId = null
        };


        var dialog = new ProductEditDialog(product, CategoryList)
        {
            XamlRoot = invoker.XamlRoot, //required to fix bug
            Title = "Add Product"
        };
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            //TODO: create clone method
            product.Name = dialog.EdittingProduct.Name;
            product.SalePrice = dialog.EdittingProduct.SalePrice;
            product.ImportPrice = dialog.EdittingProduct.ImportPrice;
            product.Quantity = dialog.EdittingProduct.Quantity;
            product.CategoryId = dialog.EdittingProduct.CategoryId;

            await _shopService.ProductService.UpsertProduct(product);
            await GetTotalPage();
            _ = SyncProducts();
        }
    

    }

    [RelayCommand(CanExecute = nameof(CanDeleteProduct))]
    private async void ProductDelete()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        await _shopService.ProductService.DeleteProduct(SelectedProduct.Id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        SelectedProduct = null;
        await GetTotalPage();
        if(SelectedPage > TotalPageCount)
        {
            SelectedPage = TotalPageCount;
        }
        else
        {
            _ = SyncProducts();
        }
    }

    [RelayCommand(CanExecute = nameof(CanEditProduct))]
    private async void ProductEdit(AppBarButton invoker)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var dialog = new ProductEditDialog(SelectedProduct, CategoryList)
        {
            XamlRoot = invoker.XamlRoot, //required to fix bug
            Title = "Edit Product"
        };
#pragma warning restore CS8604 // Possible null reference argument.
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            SelectedProduct.Name = dialog.EdittingProduct.Name;
            SelectedProduct.SalePrice = dialog.EdittingProduct.SalePrice;
            SelectedProduct.ImportPrice = dialog.EdittingProduct.ImportPrice;
            SelectedProduct.Quantity = dialog.EdittingProduct.Quantity;
            SelectedProduct.CategoryId = dialog.EdittingProduct.CategoryId;

            await _shopService.ProductService.UpsertProduct(SelectedProduct);
            _ = SyncProducts();
        }
    }
    [RelayCommand]
    private async void CategoryAdd(AppBarButton invoke)
    {
        var category = new Category()
        {
            Name = "New Category"
        };
        var dialog = new CategoryEditDialog(category)
        {
            XamlRoot = invoke.XamlRoot, //required to fix bug
            Title = "Add Category"
        };
        var result = await dialog.ShowAsync();
        if (result ==  ContentDialogResult.Primary)
        {
            category.Name = dialog.EdittingCategory.Name;
            await _shopService.ProductService.UpsertCategory(category);
            _ = SyncCategoris();
        }
    }
    [RelayCommand(CanExecute = nameof(CanDeleteCategory))]
    private async void CategoryDelete()
    {
#pragma warning disable CS8629 // CanExecute is already checked
#pragma warning disable CS8602 // CanExecute is already checked
        var catId = (int)SelectedCategory.Id;
#pragma warning restore CS8602 
#pragma warning restore CS8629 
        SelectedCategory = null;
        await _shopService.ProductService.DeleteCategory(catId);
        await SyncCategoris();
        SelectedCategory = CategoryList.Last(); //NULL category
    }
    [RelayCommand(CanExecute = nameof(CanEditCategory))]
    private async void CategoryEdit(AppBarButton invoke)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var dialog = new CategoryEditDialog(SelectedCategory)
        {
            XamlRoot = invoke.XamlRoot, //required to fix bug
            Title = "Edit Category"
        };
#pragma warning restore CS8604 // Possible null reference argument.
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            SelectedCategory.Name = dialog.EdittingCategory.Name;
            await _shopService.ProductService.UpsertCategory(SelectedCategory);
            SelectedCategory = null;
            await SyncCategoris();
            _ = SyncProducts();
        }
    }

    private bool CanDeleteProduct()
    {
        return SelectedProduct != null;
    }
    private bool CanDeleteCategory()
    {
        return SelectedCategory != null && SelectedCategory.Name != "All" && SelectedCategory.Name != "NULL";
    }
    private bool CanEditProduct()
    {
        return SelectedProduct != null;
    }
    private bool CanEditCategory()
    {
        return SelectedCategory != null && SelectedCategory.Name != "All" && SelectedCategory.Name != "NULL";
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
