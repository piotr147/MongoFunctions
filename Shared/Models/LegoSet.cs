using MongoDB.Bson.Serialization.Attributes;
using Shared.Helpers;

namespace Shared.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection("LegoSets")]
    public class LegoSet : Document
    {
        public string Number { get; set; }

        public string Year { get; set; }
        
        public string Name { get; set; }
        
        public string Series { get; set; }
        
        public string CatalogPrice { get; set; }
        
        public string Elements { get; set; }

        public bool IsRetired { get; set; }

        public override string ToString() =>
            $"{Number}, {Series} - {Name}, {Year}, {CatalogPrice} pln, {Elements} elem, {(IsRetired ? "retired" : "not-retired")}";
    }
}
