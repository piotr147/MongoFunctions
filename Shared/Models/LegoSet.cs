using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models
{
    [BsonIgnoreExtraElements]
    public class LegoSet
    {
        public string Number { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string CatalogPrice { get; set; }
        public string Elements { get; set; }
    }
}
