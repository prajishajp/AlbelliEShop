using AlbelliEShop.Core;
using AlbelliEShop.Core.Model;
using Microsoft.AspNetCore.Mvc;

namespace AlbelliEShop.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Fetches all orders and its details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(_orderService.GetAllOrders());
        }
        /// <summary>
        /// Places order and calculates the required bin width
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateOrder(OrderRequest orderRequest)
        {
            Order order = new Order();
            order.Products = new List<Core.Product>();
            foreach (var item in orderRequest.Products)
            {
                var product = new Core.Product();
                product.Quantity = item.Quantity;
                product.ProductName = item.Name;
                order.Products.Add(product);
            }
            var response = _orderService.PlaceOrder(order);
            if (response == null)
            {
                return BadRequest("Oops :( Unable to place order. Product not available");
            }
            else
            {
                var orderResponse = new OrderResponse();
                orderResponse.Id = response.Id;
                orderResponse.RequiredBinWidth = response.RequiredBinWidth;
                return Ok(orderResponse);
            }
        }
        /// <summary>
        /// Fetches order details based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult GetOrder(string id)
        {
            return Ok(_orderService.GetOrderById(id));
        }
    }
}