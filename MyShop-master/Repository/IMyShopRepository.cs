using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface IMyShopRepository
{
    ICategoryRepository Categories { get; }

    IProductRepository Products { get; }

    ICustomerRepository Customers { get; }

    IOrderRepository Orders { get; }

    IOrderDetailRepository OrderDetails { get; }


}
