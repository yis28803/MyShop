using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using App.Helpers;
using Microsoft.UI.Xaml.Data;

namespace App.Converter;
public class StringToDecimalConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal)
        {
            return value.ToString();
        }
        return "";
    }
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string s)
        {
            return s.ParseDecimal();
        }
        return null;
    }
}