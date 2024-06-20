
using Models;

namespace BusinessLogic.Interfaces;
public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAsync();

    Task<IEnumerable<Customer>> GetAsync(string search);

    Task<Customer> GetAsync(int id);


    Task<Customer> UpsertAsync(Customer customer);

    Task<IEnumerable<Order>> GetOrdersAsync(int cusId);

    Task DeleteAsync(int customerId);
}
