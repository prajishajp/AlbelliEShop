using AlbelliEShop.Domain;
using MongoDB.Driver;

namespace AlbelliEShop.Persistence
{
    public interface IDbClient
    {
        IMongoCollection<Order> GetOrdersCollection();
    }
}
