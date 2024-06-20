using System.Diagnostics;
using BusinessLogic.Interfaces;
using Models;
using Repository;

namespace BusinessLogic.Services;
public class OrderService : IOrderService
{
    private readonly IMyShopRepository _repos;

    public async Task UpsertOrder(Order order)
    {

        if (order.CustomerId == -1)
        {
            order.CustomerId = null;
        }

        var newOrder = await _repos.Orders.UpsertAsync(order);


        //TODO: convert to ForEachAsync
        foreach (var item in order.OrderDetails)
        {
            var item2 = item.Clone();
            item2.OrderId = newOrder.Id;
            try
            {
                await _repos.OrderDetails.InsertAsync(item2);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }
        }
    }
    public OrderService(IMyShopRepository repos)
    {
        _repos = repos;
    }
    public async Task DeleteAsync(int orderId)
    {
        await _repos.Orders.DeleteAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _repos.Orders.GetAsync();
    }
    public async Task<int> GetCurrentWeekOrderCountAsync()
    {
        return await _repos.Orders.GetCurrentWeekOrderCount();
    }
    public async Task<int> GetCurrentMonthOrderCountAsync()
    { 
        return await _repos.Orders.GetCurrentMonthOrderCount();
    }
    public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
    {
        return await _repos.OrderDetails.GetByOrderAsync(orderId);
    }
    public async Task<IEnumerable<DayIncome>> GetIncomeByDay(int count)
    {
        var data = await _repos.OrderDetails.GetDaysIncome();
        var sorted = new List<DayIncome>(data);

        //sort from now to past
        sorted.Sort((a, b) => b.OrderPlaced.CompareTo(a.OrderPlaced));

        //fill in missing days
        var result = new List<DayIncome>();
        var date = DateTime.Today;
        var flag = 0;
       

        while(flag < sorted.Count)
        {
            while(sorted[flag].OrderPlaced < date)
            {
                result.Insert(0,new DayIncome() { OrderPlaced = date, Profit = 0, Revenue = 0 });
                date = date.AddDays(-1);
            }
            result.Insert(0,sorted[flag]);
            date = date.AddDays(-1);
            flag++;
        }
        //var size = result.Count;
        //result.RemoveRange(0, size-count); //retain only count lastest days
        return result;
    }
    public async Task<IEnumerable<MonthIncome>> GetIncomeByMonth(int count)
    {
        //TODO: limit data by count
        var data = await _repos.OrderDetails.GetMonthsIncome();
        var sorted = new List<MonthIncome>(data);
        sorted.Sort((a, b) => b.Year==a.Year?b.Month.CompareTo(a.Month) : b.Year.CompareTo(a.Year));

        var result = new List<MonthIncome>();
        var date = DateTime.Today;
        var flag = 0;
        while(flag < sorted.Count)
        {
            while(sorted[flag].Year < date.Year || (sorted[flag].Year == date.Year && sorted[flag].Month < date.Month))
            {
                result.Insert(0, new MonthIncome() { Year = date.Year, Month = date.Month, Profit = 0, Revenue = 0 });
                date = date.AddMonths(-1);
            }
            result.Insert(0, sorted[flag]);
            date = date.AddMonths(-1);
            flag++;
        }

        return result;
    }
    public async Task<IEnumerable<WeekIncome>> GetIncomeByWeek(int count)
    {
        var data = await _repos.OrderDetails.GetWeeksIncome();
        var sorted = new List<WeekIncome>(data);

        //sort from now to past
        sorted.Sort((a, b) => b.StartOfWeek.CompareTo(a.StartOfWeek));

        //fill in missing week
        var result = new List<WeekIncome>();
        var date = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
        var flag = 0;

        while (flag < sorted.Count)
        {
            while (sorted[flag].StartOfWeek < date)
            {
                result.Insert(0, new WeekIncome() { StartOfWeek = date, Profit = 0, Revenue = 0 });
                date = date.AddDays(-7);
            }   
            result.Insert(0, sorted[flag]);
            date = date.AddDays(-7);
            flag++;
        }
        return result;
    }
    public async Task<IEnumerable<YearIncome>> GetIncomeByYear(int count)
    {
        //TODO: limit data by count
        var data = await _repos.OrderDetails.GetYearsIncome();
        var sorted = new List<YearIncome>(data);
        sorted.Sort((a, b) => b.Year.CompareTo(a.Year));
        var result = new List<YearIncome>();
        var year = DateTime.Today.Year;
        var flag = 0;
        while (flag < sorted.Count)
        {
            while (sorted[flag].Year < year)
            {
                result.Insert(0, new YearIncome() { Year = year, Profit = 0, Revenue = 0 });
                year--;
            }
            result.Insert(0, sorted[flag]);
            year--;
            flag++;
        }
        return result;
    }
    public async Task<int> GetTotalOrderCount(DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        DateTime? p1;
        if (startDate == null)
        {
            p1 = null;
        }
        else
        {
            p1 = startDate.Value.DateTime;
        }
        DateTime? p2;
        if (endDate == null)
        {
            p2 = null;
        }
        else
        {
            p2 = endDate.Value.DateTime;
        }
        return await _repos.Orders.GetTotalOrderCount(p1, p2);
    }
    public async Task<IEnumerable<Order>> QueryOrderPage(DateTimeOffset? startDate, DateTimeOffset? endDate, int pageSize, int? selectedPage)
    {
        selectedPage ??= 1;
        DateTime? p1;
        if(startDate == null)
        {
            p1 = null;
        }
        else
        {
            p1 = startDate.Value.DateTime;
        }
        DateTime? p2;
        if(endDate == null)
        {
            p2 = null;
        }
        else
        {
            p2 = endDate.Value.DateTime;
        }
        return await _repos.Orders.QueryOrderPage(p1, p2, pageSize, (int)selectedPage);
    }

    public async Task<IEnumerable<ProductSoldCount>> GetThisWeekProductSoldCountAsync(int n)
    {
        return  await _repos.OrderDetails.GetTopSellingProductsThisWeek(n);
    }
    public async Task<IEnumerable<ProductSoldCount>> GetThisYearProductSoldCountAsync(int n)
    {
        return await _repos.OrderDetails.GetTopSellingProductsThisYear(n);
    }
    public async Task<IEnumerable<ProductSoldCount>> GetThisMonthProductSoldCountAsync(int n)
    {
        return await _repos.OrderDetails.GetTopSellingProductsThisMonth(n);
    }

    public async Task<IEnumerable<ProductSoldCountDay>> GetProductSoldCountByDay(int productId)
    {
        var data = await _repos.OrderDetails.GetSoldCountDays(productId);
        var sorted = new List<ProductSoldCountDay>(data);

        //sort from now to past
        sorted.Sort((a, b) => b.Date.CompareTo(a.Date));

        //fill in missing days
        var result = new List<ProductSoldCountDay>();
        var date = DateTime.Today;
        var flag = 0;


        while (flag < sorted.Count)
        {
            while (sorted[flag].Date < date)
            {
                result.Insert(0, new ProductSoldCountDay() { Date = date, TotalQuantitySold = 0 });
                date = date.AddDays(-1);
            }
            result.Insert(0, sorted[flag]);
            date = date.AddDays(-1);
            flag++;
        }
        //var size = result.Count;
        //result.RemoveRange(0, size-count); //retain only count lastest days
        return result;
    }
    public async Task<IEnumerable<ProductSoldCountWeek>> GetProductSoldCountByWeek(int productId)
    {
        var data = await _repos.OrderDetails.GetSoldCountWeeks(productId);
        var sorted = new List<ProductSoldCountWeek>(data);

        //sort from now to past
        sorted.Sort((a, b) => b.StartOfWeek.CompareTo(a.StartOfWeek));

        //fill in missing week
        var result = new List<ProductSoldCountWeek>();
        var date = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
        var flag = 0;

        while (flag < sorted.Count)
        {
            while (sorted[flag].StartOfWeek < date)
            {
                result.Insert(0, new ProductSoldCountWeek() { StartOfWeek = date,  TotalQuantitySold = 0 });
                date = date.AddDays(-7);
            }
            result.Insert(0, sorted[flag]);
            date = date.AddDays(-7);
            flag++;
        }
        return result;
    }
    public async Task<IEnumerable<ProductSoldCountMonth>> GetProductSoldCountByMonth(int productId)
    {
        var data = await _repos.OrderDetails.GetSoldCountMonths(productId);
        var sorted = new List<ProductSoldCountMonth>(data);
        sorted.Sort((a, b) => b.Year == a.Year ? b.Month.CompareTo(a.Month) : b.Year.CompareTo(a.Year));

        var result = new List<ProductSoldCountMonth>();
        var date = DateTime.Today;
        var flag = 0;
        while (flag < sorted.Count)
        {
            while (sorted[flag].Year < date.Year || (sorted[flag].Year == date.Year && sorted[flag].Month < date.Month))
            {
                result.Insert(0, new ProductSoldCountMonth() { Year = date.Year, Month = date.Month, TotalQuantitySold = 0 });
                date = date.AddMonths(-1);
            }
            result.Insert(0, sorted[flag]);
            date = date.AddMonths(-1);
            flag++;
        }

        return result;
    }
    public async Task<IEnumerable<ProductSoldCountYear>> GetProductSoldCountByYear(int productId)
    {
        var data = await _repos.OrderDetails.GetSoldCountYears(productId);
        var sorted = new List<ProductSoldCountYear>(data);
        sorted.Sort((a, b) => b.Year.CompareTo(a.Year));
        var result = new List<ProductSoldCountYear>();
        var year = DateTime.Today.Year;
        var flag = 0;
        while (flag < sorted.Count)
        {
            while (sorted[flag].Year < year)
            {
                result.Insert(0, new ProductSoldCountYear() { Year = year, TotalQuantitySold = 0 });
                year--;
            }
            result.Insert(0, sorted[flag]);
            year--;
            flag++;
        }
        return result;
    }
}
