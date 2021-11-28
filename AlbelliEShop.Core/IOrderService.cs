using AlbelliEShop.Domain;

namespace AlbelliEShop.Core
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order PlaceOrder(Order order);
        Order GetOrderById(string id);
    }
}
