using Shared.Db;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvImporter.Import
{
    public class SetOwnershipCsvImporter : BaseCsvImporter, ISetOwnershipCsvImporter
    {
        private IDbContext<SetOwnership> _dbContext;

        public SetOwnershipCsvImporter(IDbContext<SetOwnership> dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task ImportSets(string path)
        {
            IEnumerable<SetOwnership> ownerships = await ReadSets(path);
            Console.WriteLine($"Count: {ownerships.ToArray().Length}");
            await _dbContext.InsertManyAsync(ownerships);
        }

        private async static Task<IEnumerable<SetOwnership>> ReadSets(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string line = await reader.ReadLineAsync();
            List<SetOwnership> ownerships = new List<SetOwnership>();

            while (line != null)
            {
                ownerships.Add(ReadSetOwnershipFromLine(line));

                line = await reader.ReadLineAsync();
            }

            return ownerships;
        }

        private static SetOwnership ReadSetOwnershipFromLine(string line)
        {
            string[] values = line.Split(',');

            return new SetOwnership
            {
                UserMail = GetValueOrNull(values, 0),
                SetNumber = GetValueOrNull(values, 1),
                PurchaseDate = DateTime.ParseExact(GetValueOrNull(values, 2), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                PricePln = GetIntValueOrZero(values, 3),
                PricePoints = GetIntValueOrZero(values, 4),
                WasBuilt = GetBoolValueOrFalse(values, 5),
                IsForSale = GetBoolValueOrFalse(values, 6)
            };
        }
    }
}
