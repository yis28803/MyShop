using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAsync();

    Task<Category?> GetAsync(int CategoryId);

    Task<IEnumerable<Category>> GetAsync(string search);

    Task<IEnumerable<Product>> GetProductsAsync(int Id);


    Task<Category> UpsertAsync(Category Category);

    Task DeleteAsync(int CategoryId);
}
