using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface ICustomerRepository
{
  
    Task<IEnumerable<Customer>> GetAsync();

    Task<IEnumerable<Customer>> GetAsync(string search);

    Task<Customer> GetAsync(int id);

  
    Task<Customer> UpsertAsync(Customer customer);

    Task<IEnumerable<Order>> GetOrdersAsync(int cusId);

    Task DeleteAsync(int customerId);
}
