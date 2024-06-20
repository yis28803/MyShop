using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Models;

public class ProductSoldCountDay
{
    public DateTime Date { get; set; }
    public int TotalQuantitySold { get; set; }
}

public class ProductSoldCountWeek 
{
    public DateTime StartOfWeek { get; set; }
    public int TotalQuantitySold { get; set; }
}

public class ProductSoldCountMonth 
{
    public int Month { get; set; }
    public int Year { get; set; }   
    public int TotalQuantitySold { get; set; }
}

public class ProductSoldCountYear 
{
    public int Year { get; set; }
    public int TotalQuantitySold { get; set; }
}

public class ProductSoldCount 
{
    public int ProductId { get; set; } = 0;
    public string Name { get; set; } = "";
    public int TotalQuantitySold { get; set; }
    
    public override string ToString()
    {
        return $"ProductId: {ProductId}, Name: {Name}, TotalQuantitySold: {TotalQuantitySold}";
    }
}
