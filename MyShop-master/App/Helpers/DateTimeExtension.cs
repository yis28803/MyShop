using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Helpers;

public static class DateTimeExtension{
    public static DateTime StartOfWeek(this DateTime root, int weekOffset = 0)
    {
        var startOfWeek = root.AddDays(-1 * (int)(root.DayOfWeek));
        startOfWeek.AddDays(weekOffset*7);
        return startOfWeek;
    }
    public static DateTime EndOfWeek(this DateTime root, int weekOffset = 0)
    {
        var endOfWeek = root.AddDays(7 - (int)(root.DayOfWeek));
        endOfWeek.AddDays(weekOffset*7);
        return endOfWeek;
    }

    public static DateTime StartOfMonth(this DateTime root, int monthOffset = 0)
    {
        var startOfMonth = new DateTime(root.Year, root.Month, 1);
        startOfMonth.AddMonths(monthOffset);
        return startOfMonth;
    }

    public static DateTime EndOfMonth(this DateTime root, int monthOffset = 0)
    {
        var endOfMonth = new DateTime(root.Year, root.Month, DateTime.DaysInMonth(root.Year, root.Month));
        endOfMonth.AddDays(1); //
        endOfMonth.AddMonths(monthOffset);
        endOfMonth.AddDays(-1); // last day of month
        return endOfMonth;
    }

    public static DateTime StartOfYear(this DateTime root)
    {
        var startOfYear = new DateTime(root.Year, 1, 1);
        return startOfYear;
    }

    public static DateTime EndOfYear(this DateTime root)
    {
        var endOfYear = new DateTime(root.Year, 12, 31);
        return endOfYear;
    }

    public static string GetString(this DateTime root)
    {
        return $"{root.Day}/{root.Month}/{root.Year}";
    }
}
