using MongoDB.Driver;

namespace AlbelliEShop.Core
{
    public interface IDbClient
    {
        IMongoCollection<Order> GetOrdersCollection();
    }
}
