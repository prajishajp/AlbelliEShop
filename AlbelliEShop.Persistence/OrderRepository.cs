using AlbelliEShop.Domain;
using AlbelliEShop.Persistence.Contract;
using MongoDB.Driver;

namespace AlbelliEShop.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;
        public OrderRepository(IDbClient dbClient)
        {
            _orders = dbClient.GetOrdersCollection();
        }
        public Order AddOrderInDb(Order order)
        {
            _orders.InsertOne(order);
            return order;
        }

        public List<Order> FetchAllOrdersFromDb()
        {
            return _orders.Find(order => true).ToList();
        }

        public Order FetchOrderByIdFromDb(string id)
        {
            return _orders.Find(order => order.Id == id).First();
        }
    }
}