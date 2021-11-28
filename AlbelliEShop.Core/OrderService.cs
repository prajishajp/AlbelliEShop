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
        private readonly IMongoCollection<Order> _orders;

        public OrderService(IDbClient dbClient)
        {
            _orders = dbClient.GetOrdersCollection();
        }

        public Order PlaceOrder(Order order)
        {
            bool isAValidProduct = isValidProduct(order.Products);
            if (isAValidProduct)
            {
                // calculate required bin width logic: to be added
                _orders.InsertOne(order);
                return order;
            }
            else
            {
                return null;
            }
        }

        public List<Order> GetAllOrders()
        {
            return _orders.Find(order => true).ToList();
        }
        private bool isValidProduct(List<Product> products)
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

        public Order GetOrderById(string id)
        {
            return _orders.Find(order => order.Id == id).First();
        }
    }
}