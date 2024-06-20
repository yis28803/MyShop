using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SQL;

public class CustomerRepository : ICustomerRepository
{
    private readonly MyShopContext _db;

    public CustomerRepository(MyShopContext db)
    {
        _db = db;
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _db.Customers.FirstOrDefaultAsync(_customer => _customer.Id == id);
        if (null != customer)
        {
            var orders = await _db.Orders.Where(order => order.CustomerId == id).ToListAsync();
            _db.Orders.RemoveRange(orders);
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Customer>> GetAsync()
    {
        return await _db.Customers
            .AsNoTracking()
            .ToListAsync();
    }


    public Task<IEnumerable<Customer>> GetAsync(string search)
    {
        throw new NotImplementedException();
    }

    public Task<Customer> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync(int id)
    {
        var cus = await _db.Customers.FirstOrDefaultAsync(o => o.Id == id);

        return cus?.Orders ?? Enumerable.Empty<Order>();
    }

    public async Task<Customer> UpsertAsync(Customer customer)
    {
        var current = await _db.Customers.FirstOrDefaultAsync(_customer => _customer.Id == customer.Id);
        if (null == current)
        {
            _db.Customers.Add(customer);
        }
        else
        {
            _db.Entry(current).CurrentValues.SetValues(customer);
        }
        await _db.SaveChangesAsync();
        return customer;
    }
}
