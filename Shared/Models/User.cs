using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public string Name { get; set; }
        
        public string Mail { get; set; }
    }
}
