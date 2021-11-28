using MongoDB.Bson.Serialization.Attributes;

namespace AlbelliEShop.Core
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public List<Product> Products { get; set; }
        public double RequiredBinWidthInMillimeters { get; set; }
    }
}
