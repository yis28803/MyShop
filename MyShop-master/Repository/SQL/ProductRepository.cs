using Models;
using Microsoft.EntityFrameworkCore;
using Repository.Helpers;
using System.Linq;
using System.Diagnostics;

namespace Repository.SQL;

public class ProductRepository : IProductRepository
{
    public ProductRepository(MyShopContext db)
    {
        _db = db;
    }

    private readonly MyShopContext _db;

    public async Task DeleteAsync(int productId)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (null != product)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetAsync()
    {
        return await _db.Products
        .AsNoTracking().Include(p => p.Category)
        .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAsync(string value)
    {
        return await _db.Products
        .Where(p => p.Name.Contains(value)).Include(p => p.Category)
        .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByPriceAsync(int floor, int ceil)
    {
        return await _db.Products.Where(product =>
           product.SalePrice >= floor && product.SalePrice <= ceil)
       .AsNoTracking().Include(p => p.Category)
       .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return  await _db.Products.CountAsync();
    }

    public async Task<Product> UpsertAsync(Product product)
    {
        var existingProduct = await _db.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        Debug.WriteLine($"product id: {product.Id}");
        if (existingProduct == null)
        {
            _db.Products.Add(product);
        }
        else
        {
            _db.Entry(existingProduct).CurrentValues.SetValues(product);
        }

        await _db.SaveChangesAsync();

        return product;
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _db.Products.Where(product => product.CategoryId == categoryId)
            .AsNoTracking().Include(p => p.Category)
            .ToListAsync();
    }


    public Task<Product> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> TopRunningOutOfStockAsync(int topCount)
    {
        var productsRunningOutOfStock = await _db.Products
        .OrderBy(p => p.Quantity)
        .Take(topCount).Include(p => p.Category)
        .ToListAsync();

        return productsRunningOutOfStock;
    }

    public async Task<IEnumerable<Product>> FilterProduct(ProductFilterDefinition filterDefinition)
    {
        var query = GenerateQueryByFilter(filterDefinition);

        return await query.Include(p => p.Category).ToListAsync();
    }

    public async Task<int> GetTotalProductCount(ProductFilterDefinition filterDefinition)
    {
        var query = GenerateQueryByFilter(filterDefinition);
        return await query.CountAsync();
    }

    private IQueryable<Product> GenerateQueryByFilter(ProductFilterDefinition filterDefinition)
    {
        var query = from s in _db.Products
                        select s;
        
        //id = 0 means all categories
        if (filterDefinition.CategoryId != 0)
        {
            query = query.Where(p => p.CategoryId == filterDefinition.CategoryId);
        }
        if(filterDefinition.Name != null && filterDefinition.Name != "")
        {
            query = query.Where(p => p.Name.Contains(filterDefinition.Name));
        }
        if(filterDefinition.LowSalePrice != null)
        {
            query = query.Where(p => p.SalePrice >= filterDefinition.LowSalePrice);
        }
        if(filterDefinition.HighSalePrice != null)
        {
            query = query.Where(p => p.SalePrice <= filterDefinition.HighSalePrice);
        }
       
        switch(filterDefinition.SortOder)
        {
            case ProductSortingEnum.IdAsc:
                query = query.OrderBy(p => p.Id);
                break;
            case ProductSortingEnum.IdDesc:
                query = query.OrderByDescending(p => p.Id);
                break;
            case ProductSortingEnum.NameAsc:
                query = query.OrderBy(p => p.Name);
                break;
            case ProductSortingEnum.NameDesc:
                query = query.OrderByDescending(p => p.Name);
                break;
            case ProductSortingEnum.SalePriceAsc:
                query = query.OrderBy(p => p.SalePrice);
                break;
            case ProductSortingEnum.SalePriceDesc:
                query = query.OrderByDescending(p => p.SalePrice);
                break;
            case ProductSortingEnum.QuantityAsc:
                query = query.OrderBy(p => p.Quantity);
                break;
            case ProductSortingEnum.QuantityDesc:
                query = query.OrderByDescending(p => p.Quantity);
                break;
            default:
                break;
        }

        if (filterDefinition.PageSize != null && filterDefinition.PageNumber != null)
        {
            var position = (int)((filterDefinition.PageNumber - 1) * filterDefinition.PageSize);
            query = query.Skip(position).Take((int)filterDefinition.PageSize);
        }

        return query;
    }
}
