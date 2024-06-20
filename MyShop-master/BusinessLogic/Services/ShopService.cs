using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.SQL;

namespace BusinessLogic.Services;
public class ShopService : IShopService
{
    private readonly IMyShopRepository _shopRepository;

    public ShopService(string server, string database, string? username, string? password, bool isTrusted)
    {
        DbContextOptionsBuilder<MyShopContext>  dbOptionsBuilder;
        if (password == null)
        {
            dbOptionsBuilder = new DbContextOptionsBuilder<MyShopContext>()
            .UseSqlServer($"Server={server};Database={database};Trusted_Connection={isTrusted};");
            Debug.WriteLine("Connect By Trusted Connection");
        }
        else
        {
            dbOptionsBuilder = new DbContextOptionsBuilder<MyShopContext>()
            .UseSqlServer($"Server={server};Database={database};User Id={username};Password={password};"); /*Trusted_Connection={isTrusted};*/
            Debug.WriteLine("Connect By Account");
        }



        _shopRepository = new MyShopRepository(dbOptionsBuilder);

    }

    //TODO: consider pass _shopRepository by reference in following initialization
    public IProductService ProductService => new ProductService(_shopRepository);

    public ICustomerService CustomerService => new CustomerSevice(_shopRepository);

    public IOrderService OrderService => new OrderService(_shopRepository);
}