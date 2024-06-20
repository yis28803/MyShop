using Microsoft.EntityFrameworkCore;


namespace Repository.SQL;

public class MyShopRepository : IMyShopRepository
{
    private readonly DbContextOptions<MyShopContext> _dbOptions;

    public MyShopRepository(DbContextOptionsBuilder<MyShopContext>
        dbOptionsBuilder)
    {
        _dbOptions = dbOptionsBuilder.Options;
        using var db = new MyShopContext(_dbOptions);
        db.Database.EnsureCreated();
    }

    public ICategoryRepository Categories => new CategoryRepository(new MyShopContext(_dbOptions));

    public IProductRepository Products => new ProductRepository(new MyShopContext(_dbOptions));

    public ICustomerRepository Customers => new CustomerRepository(new MyShopContext(_dbOptions));

    public IOrderRepository Orders => new OrderRepository(new MyShopContext(_dbOptions));

    public IOrderDetailRepository OrderDetails => new OrderDetailRepository(new MyShopContext(_dbOptions));
}
