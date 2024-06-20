using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Models;
using App.Converter;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Dialog;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProductEditDialog : ContentDialog
{
    public readonly Collection<Category> CategoryList;
    public Product EdittingProduct;
    public ProductEditDialog(Product selected, Collection<Category> categories)
    {
        InitializeComponent();
        EdittingProduct = new()
        {
            Name = selected.Name,
            SalePrice = selected.SalePrice,
            ImportPrice = selected.ImportPrice,
            Quantity = selected.Quantity,
            CategoryId = selected.CategoryId
        };

        Binding binding = new()
        {
            Source = EdittingProduct,
            Path = new PropertyPath("Name"),
            Mode = BindingMode.TwoWay
        };
        ProductNameTextBox.SetBinding(TextBox.TextProperty, binding);

        binding = new Binding()
        {
            Source = EdittingProduct,
            Path = new PropertyPath("SalePrice"),
            Converter = new StringToDecimalConverter(),
            Mode = BindingMode.TwoWay
        };
        SalePriceTextBox.SetBinding(TextBox.TextProperty, binding);

        binding = new Binding()
        {
            Source = EdittingProduct,
            Path = new PropertyPath("ImportPrice"),
            Converter = new StringToDecimalConverter(),
            Mode = BindingMode.TwoWay
        };
        ImportPriceTextBox.SetBinding(TextBox.TextProperty, binding);

        binding = new Binding()
        {
            Source = EdittingProduct,
            Path = new PropertyPath("Quantity"),
            Mode = BindingMode.TwoWay
        };
        QuantityTextBox.SetBinding(TextBox.TextProperty, binding);

        CategoryList = new();
        for (var i = 1; i < categories.Count; i++)
        {
            CategoryList.Add(categories[i]);
        }
        CategoryComboBox.ItemsSource = CategoryList;
        CategoryComboBox.DisplayMemberPath = "Name";
        CategoryComboBox.SelectedItem = CategoryList.FirstOrDefault(x => x.Id == selected.CategoryId);
        CategoryComboBox.SelectionChanged += OnCategorySelectionChanged;
    }

    private void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CategoryComboBox.SelectedItem is Category category)
        {
            EdittingProduct.CategoryId = category.Id;
        }
    }
}
