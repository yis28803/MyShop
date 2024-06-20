using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Helpers;

public static class StringExtensions{
    public static decimal? ParseDecimal(this string? value)
    {
        return string.IsNullOrEmpty(value) ? null : decimal.Parse(value);
    }
    public static int? ParseInt(this string? value)
    {
        return string.IsNullOrEmpty(value) ? null : int.Parse(value);
    }
    public static DateTime ToDate(this string dateFormated)
    {
        return DateTime.Parse(dateFormated);
    }
}
