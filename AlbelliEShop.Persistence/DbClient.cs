using AlbelliEShop.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AlbelliEShop.Persistence
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Order> _orders;
        public DbClient(IOptions<DbConfig> dbConfig)
        {
            var client = new MongoClient(dbConfig.Value.Connection_String);
            var database = client.GetDatabase(dbConfig.Value.Database_Name);
            _orders = database.GetCollection<Order>(dbConfig.Value.Collection_Name);
        }

        public IMongoCollection<Order> GetOrdersCollection() => _orders;

    }
}
