using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces;
public interface IShopService
{
    IProductService ProductService { get; }

    ICustomerService CustomerService { get; }

    IOrderService OrderService { get; }
}
