using System;

namespace Shared.Models
{
    public class OwnedSet
    {
        public string Number { get; set; }

        public string Year { get; set; }

        public string Name { get; set; }

        public string Series { get; set; }

        public string CatalogPrice { get; set; }

        public string Elements { get; set; }

        public bool IsRetired { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int PricePln { get; set; }

        public int PricePoints { get; set; }

        public bool WasBuilt { get; set; }

        public bool IsForSale { get; set; }

        public OwnedSet(LegoSet set, SetOwnership own)
        {
            Number = set.Number;
            Year = set.Year;
            Name = set.Name;
            Series = set.Series;
            CatalogPrice = set.CatalogPrice;
            Elements = set.Elements;
            IsRetired = set.IsRetired;
            PurchaseDate = own.PurchaseDate;
            PricePln = own.PricePln;
            PricePoints = own.PricePoints;
            WasBuilt = own.WasBuilt;
            IsForSale = own.IsForSale;
        }
    }
}
