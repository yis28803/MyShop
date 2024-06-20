using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Repository.Helpers;
public class ProductFilterDefinition
{
    public ProductFilterDefinition(ProductSortingEnum sort, int? categorId = null, string? name = null, decimal? lowSalePrice = null, decimal? highSalePrice = null, int? pageSize = null, int? pageIndex = null)
    {
        SortOder = sort;
        CategoryId = categorId;
        Name = name;
        LowSalePrice = lowSalePrice;
        HighSalePrice = highSalePrice;
        PageSize = pageSize;
        PageNumber = pageIndex;
    }

    //filtering
    public int? CategoryId { get; set; }
    public string? Name { get; set; }
    public decimal? LowSalePrice { get; set;}
    public decimal? HighSalePrice { get; set;}

    //sorting
    public ProductSortingEnum SortOder { get; set; }

    //paging
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}
