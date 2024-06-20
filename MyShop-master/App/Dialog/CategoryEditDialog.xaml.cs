using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Models;

//TODO: Remove to Views folder
namespace App.Dialog;
public sealed partial class CategoryEditDialog : ContentDialog
{
    public Category EdittingCategory;
    public CategoryEditDialog(Category selected)
    {
        InitializeComponent();

        EdittingCategory = new()
        {
            Name = selected.Name,
        };
        Binding binding = new()
        {
            Source = EdittingCategory,
            Path = new PropertyPath("Name"),
            Mode = BindingMode.TwoWay
        };

        CategoryNameTextBox.SetBinding(TextBox.TextProperty, binding);
    }
}

