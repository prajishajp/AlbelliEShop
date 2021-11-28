namespace AlbelliEShop.Core.Model
{
    public class OrderRequest
    {
        public List<Product> Products { get; set; }
    }
    public class Product
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
