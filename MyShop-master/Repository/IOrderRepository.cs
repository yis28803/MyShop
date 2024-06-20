using Models;

namespace Repository;

public interface IOrderRepository
{
  
    Task<IEnumerable<Order>> GetAsync();
    public  Task<Order?> GetAsync(int id);
    Task<Order> UpsertAsync(Order order);
    Task DeleteAsync(int orderId);
    Task<IEnumerable<Order>> GetByDateAsync(DateTime start, DateTime end);
    Task<int> GetCurrentWeekOrderCount();
    Task<int> GetCurrentMonthOrderCount();
    Task<IEnumerable<Order>> QueryOrderPage(DateTime? startDate, DateTime? endDate, int pageSize, int selectedPage);
    Task<int> GetTotalOrderCount(DateTime? startDate, DateTime? endDate);
}
