using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface IOrderDetailRepository
{
    
    Task<IEnumerable<OrderDetail>> GetByOrderAsync(int orderId);

    Task<OrderDetail?> GetAsync(int id);

    Task<OrderDetail> UpsertAsync(OrderDetail record);

    Task DeleteAsync(int id);

    Task DeleteByOrder(int orderId);

    Task InsertAsync(OrderDetail orderDetail);

    Task<IEnumerable<DayIncome>> GetDaysIncome();
    Task<IEnumerable<WeekIncome>> GetWeeksIncome();
    Task<IEnumerable<MonthIncome>> GetMonthsIncome();
    Task<IEnumerable<YearIncome>> GetYearsIncome();

    Task<IEnumerable<ProductSoldCountDay>> GetSoldCountDays(int productId);
    Task<IEnumerable<ProductSoldCountWeek>> GetSoldCountWeeks(int productId);
    Task<IEnumerable<ProductSoldCountMonth>> GetSoldCountMonths(int productId);
    Task<IEnumerable<ProductSoldCountYear>> GetSoldCountYears(int productId);

    Task<IEnumerable<ProductSoldCount>> GetTopSellingProductsThisWeek(int count);
    Task<IEnumerable<ProductSoldCount>> GetTopSellingProductsThisMonth(int count);
    Task<IEnumerable<ProductSoldCount>> GetTopSellingProductsThisYear(int count);
}
