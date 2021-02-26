using MongoDB.Bson.Serialization.Attributes;
using Shared.Helpers;

namespace Shared.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection("Users")]
    public class User : Document
    {
        public string Name { get; set; }
        
        public string Mail { get; set; }
    }
}
