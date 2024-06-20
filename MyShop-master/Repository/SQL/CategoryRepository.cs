using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SQL;

public class CategoryRepository : ICategoryRepository
{
    private readonly MyShopContext _db;

    public CategoryRepository(MyShopContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task DeleteAsync(int categoryId)
    {
        var categoryToDelete = await _db.Categories.FindAsync(categoryId);

        if (categoryToDelete != null)
        {
            _db.Categories.Remove(categoryToDelete);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Category>> GetAsync()
    {
        return await _db.Categories.Include(c=>c.Products).ToListAsync();
    }

    public async Task<Category?> GetAsync(int categoryId)
    {
        return await _db.Categories.FindAsync(categoryId);
    }

    public async Task<IEnumerable<Category>> GetAsync(string search)
    {
        return await _db.Categories
            .Where(c => c.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(int Id)
    {
        var category = await _db.Categories
           //.Include(o => o.OrderDetails
           .FirstOrDefaultAsync(o => o.Id == Id);

        return category?.Products ?? Enumerable.Empty<Product>();

    }

    public async Task<Category> UpsertAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        var existingCategory = await _db.Categories.FindAsync(category.Id);

        if (existingCategory == null)
        {
            _db.Categories.Add(category);
        }
        else
        {
            _db.Entry(existingCategory).CurrentValues.SetValues(category);
        }

        await _db.SaveChangesAsync();

        return category;
    }
}
