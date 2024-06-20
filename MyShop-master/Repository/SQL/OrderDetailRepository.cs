using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SQL;

public class OrderDetailRepository : IOrderDetailRepository
{

    private readonly MyShopContext _db;

    public OrderDetailRepository(MyShopContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task DeleteAsync(int id)
    {
        var orderDetailToDelete = await _db.OrderDetails.FindAsync(id);

        if (orderDetailToDelete != null)
        {
            _db.OrderDetails.Remove(orderDetailToDelete);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OrderDetail>> GetByOrderAsync(int orderId)
    {
        return await _db.OrderDetails
            .Where(o => o.OrderId == orderId).Include(o => o.Product)
            .ToListAsync();
    }

    public async Task<OrderDetail?> GetAsync(int id)
    {
        return await _db.OrderDetails.Include(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<OrderDetail> UpsertAsync(OrderDetail record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        var existingOrderDetail = await _db.OrderDetails.FindAsync(record.OrderId);

        if (existingOrderDetail == null)
        {
            Debug.WriteLine(record);
            _db.OrderDetails.Add(record); //??
        }
        else
        {
            _db.Entry(existingOrderDetail).CurrentValues.SetValues(record);
        }

        await _db.SaveChangesAsync();

        return record;
    }

    public Task DeleteByOrder(int orderId) => throw new NotImplementedException();
    public async Task InsertAsync(OrderDetail orderDetail)
    {
        await _db.OrderDetails.AddAsync(orderDetail);

        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductSoldCount>> GetTopSellingProductsThisWeek(int count)
    {
        //TODO: alter procedure to accept count
        return await _db.ProductSoldCounts.FromSqlRaw("exec GetTopSellingProductsCurrentWeek").ToListAsync();
        
    }
    public async Task<IEnumerable<ProductSoldCount>> GetTopSellingProductsThisMonth(int count)
    { //TODO: alter procedure to accept count
        return await _db.ProductSoldCounts.FromSqlRaw("exec GetTopSellingProductsCurrentMonth").ToListAsync();
    }
    public async Task<IEnumerable<ProductSoldCount>> GetTopSellingProductsThisYear(int count)
    { //TODO: alter procedure to accept count
        return await _db.ProductSoldCounts.FromSqlRaw("exec GetTopSellingProductsCurrentYear").ToListAsync();
    }

    public async Task<IEnumerable<DayIncome>> GetDaysIncome()
    {
        return await _db.DayIncomes.FromSqlRaw("exec GetRevenueAndProfitByDay").ToListAsync();
    }
    public async Task<IEnumerable<WeekIncome>> GetWeeksIncome()
    {
        return await _db.WeekIncomes.FromSqlRaw("exec GetRevenueAndProfitByWeek").ToListAsync();
    }
    public async Task<IEnumerable<MonthIncome>> GetMonthsIncome()
    {
        return await _db.MonthIncomes.FromSqlRaw("exec GetRevenueAndProfitByMonth").ToListAsync();
    }
    public async Task<IEnumerable<YearIncome>> GetYearsIncome()
    {
        return await _db.YearIncomes.FromSqlRaw("exec GetRevenueAndProfitByYear").ToListAsync();
    }
    public async Task<IEnumerable<ProductSoldCountDay>> GetSoldCountDays(int productId)
    {
        return await _db.ProductSoldCountDays.FromSqlRaw("exec GetProductSoldCountByDay {0}", productId).ToListAsync();
    }
    public async Task<IEnumerable<ProductSoldCountWeek>> GetSoldCountWeeks(int productId)
    {
        return await _db.ProductSoldCountWeeks.FromSqlRaw("exec GetProductSoldCountByWeek {0}", productId).ToListAsync();
    }
    public async Task<IEnumerable<ProductSoldCountMonth>> GetSoldCountMonths(int productId)
    {
        return await _db.ProductSoldCountMonths.FromSqlRaw("exec GetProductSoldCountByMonth {0}", productId).ToListAsync();
    }
    public async Task<IEnumerable<ProductSoldCountYear>> GetSoldCountYears(int productId)
    {
        return await _db.ProductSoldCountYears.FromSqlRaw("exec GetProductSoldCountByYear {0}", productId).ToListAsync();
    }
}
