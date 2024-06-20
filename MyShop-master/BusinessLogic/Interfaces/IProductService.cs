using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repository.Helpers;

namespace BusinessLogic.Interfaces;

public interface IProductService
{
    IEnumerable<string> ProductSortTypes { get; }
    Task<IEnumerable<Category>> GetCategories();
    Task UpsertCategory(Category Category);
    Task DeleteCategory(int CategoryId);
    Task UpsertProduct(Product product);
    Task DeleteProduct(int productId);
    Task<IEnumerable<Product>> QueryProductPage(Category? c = null, string? name = null, decimal? lowPrice = null, decimal? highPrice = null, string? sort = "Id Asc", int? pageSize = 10, int? pageNumber = 1);
    Task<int> GetTotalProductCountAsync(); 
    Task<int> GetTotalProductCountAsync(Category? c = null, string? name = null, decimal? lowPrice = null, decimal? highPrice = null);
    Task<IEnumerable<Product>> GetTopRunningOutOfStockAsync(int topCount);

    Task<IEnumerable<Product>> GetTopSellingProductsCurrentWeek();
    Task<IEnumerable<Product>> GetTopSellingProductsCurrentMonth();
    Task<IEnumerable<Product>> GetTopSellingProductsCurrentYear();
    Task<IEnumerable<Product>> Search(string? search);
    Task<IEnumerable<Product>> GetAll();
}
