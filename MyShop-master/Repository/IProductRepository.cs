using Microsoft.EntityFrameworkCore.Query.Internal;
using Models;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface IProductRepository
{


    Task<IEnumerable<Product>> GetAsync();

    Task<Product> GetAsync(int id);

    Task<IEnumerable<Product>> GetAsync(string value);

    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryid);

    Task<int> GetTotalCountAsync();

    Task<IEnumerable<Product>> TopRunningOutOfStockAsync(int topCount);

 
    Task<Product> UpsertAsync(Product product);

    Task DeleteAsync(int productId);

    Task<IEnumerable<Product>> GetByPriceAsync(int floor, int ceil);

    Task<int> GetTotalProductCount(ProductFilterDefinition filterDefinition);
    Task<IEnumerable<Product>> FilterProduct(ProductFilterDefinition filterDefinition); 
}
