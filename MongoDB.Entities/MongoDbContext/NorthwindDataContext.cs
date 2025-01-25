using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities.Entities;
using MongoDB.Entities.Helpers;

namespace MongoDB.Entities.MongoDbContext
{
    public class NorthwindDataContext
    {
        private readonly IMongoDatabase _database;

        public NorthwindDataContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("NorthwindDb"));
            _database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>(CollectionNames.Products);
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>(CollectionNames.Orders);
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>(CollectionNames.Categories);
        public IMongoCollection<OrderDetail> OrderDetails => _database.GetCollection<OrderDetail>(CollectionNames.OrderDetails);
        public IMongoCollection<Shipper> Shippers => _database.GetCollection<Shipper>(CollectionNames.Shippers);
        public IMongoCollection<Supplier> Suppliers => _database.GetCollection<Supplier>(CollectionNames.Suppliers);
        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>(CollectionNames.Customers);
        public IMongoCollection<Employee> Employees => _database.GetCollection<Employee>(CollectionNames.Employees);
        public IMongoCollection<Territory> Territories => _database.GetCollection<Territory>(CollectionNames.Territories);
        public IMongoCollection<Region> Regions => _database.GetCollection<Region>(CollectionNames.Regions);
        public IMongoCollection<EmployeeTerritory> EmployeeTerritories => _database.GetCollection<EmployeeTerritory>(CollectionNames.EmployeeTerritories);
    }
}
