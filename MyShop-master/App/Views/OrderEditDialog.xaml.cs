
using Microsoft.UI.Xaml.Controls;
using App.ViewModels;
using Models;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Views;

public sealed partial class OrderEditDialog : ContentDialog
{
    public OrderEditViewModel ViewModel { get; } 

    
    public OrderEditDialog(Order editting)
    {
        InitializeComponent();
        ViewModel = App.GetService<OrderEditViewModel>();
        _ = ViewModel.InitializeProperties(editting);
    }

    public void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
         ViewModel.SyncProductList();
    }

}
