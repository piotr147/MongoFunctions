using MongoDB.Bson.Serialization.Attributes;
using Shared.Helpers;
using System;

namespace Shared.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection("SetOwnerships")]
    public class SetOwnership : Document
    {
        public string UserMail { get; set; }

        public string SetNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int PricePln { get; set; }

        public int PricePoints { get; set; }

        public bool WasBuilt { get; set; }

        public bool IsForSale { get; set; }
    }
}
