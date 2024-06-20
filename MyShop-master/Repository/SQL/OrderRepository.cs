using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SQL;

public class OrderRepository : IOrderRepository
{
    private readonly MyShopContext _db;

    public OrderRepository(MyShopContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task DeleteAsync(int orderId)
    {
        var orderToDelete = await _db.Orders.FindAsync(orderId);

        if (orderToDelete != null)
        {
            _db.Orders.Remove(orderToDelete);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Order>> GetAsync()
    {
        return await _db.Orders
            .ToListAsync();
    }

    public async Task<Order?> GetAsync(int id)
    {
        return await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetByDateAsync(DateTime start, DateTime end)
    {
        return await _db.Orders
            .Where(o => o.OrderPlaced >= start && o.OrderPlaced <= end)
            
            .ToListAsync();
    }

    public async Task<int> GetCurrentMonthOrderCount()
    {
        //call stored procedure
        return await _db.Orders
            .Where(o => o.OrderPlaced.Month == DateTime.Now.Month)
            .CountAsync();
    }

    public async Task<int> GetCurrentWeekOrderCount()
    {
        var startWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
        return await _db.Orders.Where(o => o.OrderPlaced >= startWeek).CountAsync();
    }


    public async Task<int> GetTotalMonthOrder()
    {
        return await _db.Orders
            .Where(o => o.OrderPlaced.Month == DateTime.Now.Month)
            .CountAsync();
    }

    public async Task<int> GetTotalOrderCount(DateTime? startDate, DateTime? endDate)
    {
        var query = _db.Orders
            .Where(od => (!startDate.HasValue || od.OrderPlaced >= startDate) &&
                            (!endDate.HasValue || od.OrderPlaced <= endDate));
        return await query.CountAsync();
    }
    public Task<int> GetTotalWeekOrder() => throw new NotImplementedException();

    public async Task<IEnumerable<Order>> QueryOrderPage(DateTime? startDate, DateTime? endDate, int pageSize, int selectedPage)
    {
        var query = _db.Orders
            .Where(o => (!startDate.HasValue || o.OrderPlaced >= startDate) && (!endDate.HasValue || o.OrderPlaced <= endDate));
        var position = (selectedPage - 1) * pageSize;
        return await query.OrderByDescending(o => o.OrderPlaced).Skip(position).Take(pageSize).Include(o => o.Customer).Include(o=>o.OrderDetails).ToListAsync();
    }

  

    //TODO: use transaction to ensure data integrity
    public async Task<Order> UpsertAsync(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        var existingOrder = await _db.Orders.FindAsync(order.Id);

        if (existingOrder != null)
        {

            //delete and re-insert order
            _db.Orders.Remove(existingOrder); //delete cascade was defined in database
            await _db.SaveChangesAsync();
            
        }
        _db.Orders.Add(order.Clone());
        await _db.SaveChangesAsync();

        return _db.Orders.FirstOrDefault(o => o.CustomerId == order.CustomerId && o.OrderPlaced == order.OrderPlaced);
    }

}
