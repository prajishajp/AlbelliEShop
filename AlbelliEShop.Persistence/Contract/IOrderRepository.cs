using AlbelliEShop.Domain;

namespace AlbelliEShop.Persistence.Contract
{
    public interface IOrderRepository
    {
        Order AddOrderInDb(Order order);
        List<Order> FetchAllOrdersFromDb();
        Order FetchOrderByIdFromDb(string id);
    }
}
