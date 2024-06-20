using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class IncomeBase
{
    public decimal Revenue { get; set; } = 0;
    public decimal Profit { get; set; } = 0;
}
public class DayIncome : IncomeBase
{
    public DateTime OrderPlaced { get; set; }
}

public class WeekIncome : IncomeBase
{
    public DateTime StartOfWeek { get; set; }
}

public class MonthIncome : IncomeBase
{
    public int Month { get; set; }
    public int Year { get; set; }
}

public class YearIncome : IncomeBase
{
    public int Year { get; set; }
}