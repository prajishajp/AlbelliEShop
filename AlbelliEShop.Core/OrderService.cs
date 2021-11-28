using AlbelliEShop.Domain;
using AlbelliEShop.Persistence.Contract;
using MongoDB.Driver;

namespace AlbelliEShop.Core
{
    public class OrderService : IOrderService
    {
        List<ProductCatalog> productCatalog = new List<ProductCatalog>()
        {
            new ProductCatalog{ Name = "photobook", WidthInMillimeters = 19},
            new ProductCatalog{ Name = "calendar", WidthInMillimeters = 10},
            new ProductCatalog{ Name = "canvas", WidthInMillimeters = 16},
            new ProductCatalog{ Name = "cards", WidthInMillimeters = 4.7},
            new ProductCatalog{ Name = "mug", WidthInMillimeters = 94}
        };
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order PlaceOrder(Order order)
        {
            try
            {
                bool isAValidProduct = CheckProductValidity(order.Products);
                if (isAValidProduct)
                {
                    order.RequiredBinWidthInMillimeters = CalculateBinWidth(order.Products);
                    var placedOrder = _orderRepository.AddOrderInDb(order);
                    return placedOrder;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<Order> GetAllOrders()
        {
            var allOrders = _orderRepository.FetchAllOrdersFromDb();
            return allOrders;
        }
        public Order GetOrderById(string id)
        {
            var order = _orderRepository.FetchOrderByIdFromDb(id);
            return order;
        }

        #region Private Methods
        private bool CheckProductValidity(List<Product> products)
        {
            foreach (var product in products)
            {
                if (productCatalog.Where(x => x.Name.Equals(product.ProductName.ToLower())).ToList().Count == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private double CalculateBinWidth(List<Product> products)
        {
            double requiredBinWidthInMillimeters = 0;

            foreach (Product product in products)
            {
                double productBinWidth = productCatalog.Where(x => x.Name == product.ProductName.ToLower()).First().WidthInMillimeters;
                if (product.ProductName.ToLower() == "mug")
                {
                    int mugWidthQuantity = 0;
                    if ((int)product.Quantity % 4 == 0)
                    {
                        mugWidthQuantity = ((int)product.Quantity / 4);
                    }
                    else
                        mugWidthQuantity = ((int)product.Quantity / 4) + 1;

                    requiredBinWidthInMillimeters += productBinWidth * mugWidthQuantity;
                }
                else
                    requiredBinWidthInMillimeters += productBinWidth * product.Quantity;
            }

            return requiredBinWidthInMillimeters;
        }
        #endregion
    }
}