﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace App.Converter;
public class DateTimeToDateTimeOffsetConverter: IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            return new DateTimeOffset((DateTime)value);
        }
        return null;
    }
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dto)
        {
            return dto.DateTime;
        }
        return null;
    }
}