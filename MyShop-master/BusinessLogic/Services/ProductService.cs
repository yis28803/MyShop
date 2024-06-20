using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Models;
using Repository;
using Repository.Helpers;
using Repository.SQL;

namespace BusinessLogic.Services;
public class ProductService : IProductService
{
    private readonly IMyShopRepository _repos;
    public ProductService(IMyShopRepository repos)
    {
        _repos = repos;
    }

    public IEnumerable<string> ProductSortTypes => new List<string> { "Id Asc", "Id Desc", "Name Asc", "Name Desc", "Sale Price Asc", "Sale Price Desc", "Quantity Asc", "Quantity Desc" };

    public async Task DeleteCategory(int CategoryId)
    {
        await _repos.Categories.DeleteAsync(CategoryId);
    }
    public async Task DeleteProduct(int productId)
    {
        await _repos.Products.DeleteAsync(productId);
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _repos.Products.GetAsync();
    }

    public async Task<IEnumerable<Category>> GetCategories() {
        var raw = await _repos.Categories.GetAsync();
    
        var list = new List<Category> { new() {Id = 0, Name = "All" } };
        list.AddRange(raw);
        list.Add(new() {Id=null, Name = "NULL"});
        return list;
    }
    public async Task<IEnumerable<Product>> GetTopRunningOutOfStockAsync(int topCount)
    {
        return await _repos.Products.TopRunningOutOfStockAsync(topCount);
    }

    public Task<IEnumerable<Product>> GetTopSellingProductsCurrentMonth() => throw new NotImplementedException();
    public Task<IEnumerable<Product>> GetTopSellingProductsCurrentWeek() => throw new NotImplementedException();
    public Task<IEnumerable<Product>> GetTopSellingProductsCurrentYear() => throw new NotImplementedException();

    public async Task<int> GetTotalProductCountAsync()
    {
        return await _repos.Products.GetTotalCountAsync();
    }

    public async Task<int> GetTotalProductCountAsync(Category? c = null, string? name = null, decimal? lowPrice = null, decimal? highPrice = null)
    {
        if (c == null)
        {
            return 0;
        }
        return await _repos.Products.GetTotalProductCount(new ProductFilterDefinition(ProductSortingEnum.IdAsc, c.Id, name, lowPrice, highPrice));
    }

    public async Task<IEnumerable<Product>> QueryProductPage(Category? c = null, string? name = null, decimal? lowSalePrice = null, decimal? highSalePrice = null, string? sort = "Id Asc", int? pageSize = 10, int? pageNumber = 1)
    {
        if(c==null){ 
            return new List<Product>();
        }

        var sortEnum = sort switch
        {
            "Id Asc" => ProductSortingEnum.IdAsc,
            "Id Desc" => ProductSortingEnum.IdDesc,
            "Name Asc" => ProductSortingEnum.NameAsc,
            "Name Desc" => ProductSortingEnum.NameDesc,
            "Sale Price Asc" => ProductSortingEnum.SalePriceAsc,
            "Sale Price Desc" => ProductSortingEnum.SalePriceDesc,
            "Quantity Asc" => ProductSortingEnum.QuantityAsc,
            "Quantity Desc" => ProductSortingEnum.QuantityDesc,
            _ => ProductSortingEnum.IdAsc,
        };
        
        pageSize ??= 10;
        pageNumber ??= 1;

        var productFilterDefinition = new ProductFilterDefinition(sortEnum, c.Id, name, lowSalePrice, highSalePrice, pageSize, pageNumber);

        return await _repos.Products.FilterProduct(productFilterDefinition);
    }

    public async Task<IEnumerable<Product>> Search(string? search)
    {
        if(search == null)
        {
            return await _repos.Products.GetAsync();
        }
        return await _repos.Products.GetAsync(search);
    }

    public async Task UpsertCategory(Category Category)
    {
        //TODO: validate
        await _repos.Categories.UpsertAsync(Category);
    }
    public async Task UpsertProduct(Product product)
    {
        //TODO: validate
        await _repos.Products.UpsertAsync(product);
    }
}
