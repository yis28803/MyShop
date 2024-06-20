using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace BusinessLogic.Interfaces;
public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();

    // delete order and all order details
    Task DeleteAsync(int orderId);
    
    // update or insert. if update, delete all order details and re-insert
    Task UpsertOrder(Order order);
    Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId);
    Task<int> GetCurrentWeekOrderCountAsync();
    Task<int> GetCurrentMonthOrderCountAsync();

    Task<IEnumerable<ProductSoldCount>> GetThisWeekProductSoldCountAsync(int n);
    Task<IEnumerable<ProductSoldCount>> GetThisYearProductSoldCountAsync(int n);
    Task<IEnumerable<ProductSoldCount>> GetThisMonthProductSoldCountAsync(int n);

    /// <param name="count"> number of lastest day income will be include to the result </param>
    Task<IEnumerable<DayIncome>> GetIncomeByDay(int count);

    /// <param name="count"> number of lastest week income will be include to the result </param>
    Task<IEnumerable<WeekIncome>> GetIncomeByWeek(int count);

    /// <param name="count"> number of lastest month income will be include to the result </param>
    Task<IEnumerable<MonthIncome>> GetIncomeByMonth(int count);

    /// <param name="count"> number of lastest year income will be include to the result </param>
    Task<IEnumerable<YearIncome>> GetIncomeByYear(int count);


    // get total quantity of a product saled
    Task<int> GetTotalOrderCount(DateTimeOffset? startDate, DateTimeOffset? endDate);

    // get orders by date, pagesize, page
    Task<IEnumerable<Order>> QueryOrderPage(DateTimeOffset? startDate, DateTimeOffset? endDate, int pageSize, int? selectedPage);
    Task<IEnumerable<ProductSoldCountDay>> GetProductSoldCountByDay(int productId);
    Task<IEnumerable<ProductSoldCountWeek>> GetProductSoldCountByWeek(int productId);
    Task<IEnumerable<ProductSoldCountMonth>> GetProductSoldCountByMonth(int productId);
    Task<IEnumerable<ProductSoldCountYear>> GetProductSoldCountByYear(int productId);
}
