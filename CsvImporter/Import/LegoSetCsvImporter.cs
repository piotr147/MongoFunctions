using Shared.Db;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvImporter.Import
{
    public class LegoSetCsvImporter : BaseCsvImporter, ILegoSetCsvImporter
    {
        private IDbContext<LegoSet> _dbContext;

        public LegoSetCsvImporter(IDbContext<LegoSet> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ImportSets(string path)
        {
            IEnumerable<LegoSet> sets = await ReadSets(path);
            Console.WriteLine($"Number of sets to insert: {sets.ToArray().Length}");
            await UpsertSets(sets);
        }

        private async static Task<IEnumerable<LegoSet>> ReadSets(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string line = await reader.ReadLineAsync();
            List<LegoSet> sets = new List<LegoSet>();

            while (line != null)
            {
                sets.Add(ReadSetFromLine(line));

                line = await reader.ReadLineAsync();
            }

            return sets;
        }

        private static LegoSet ReadSetFromLine(string line)
        {
            string[] values = line.Split(',');

            return new LegoSet
            {
                Number = GetValueOrNull(values, 0),
                Year = GetValueOrNull(values, 1),
                Series = GetValueOrNull(values, 2),
                Name = GetValueOrNull(values, 3),
                CatalogPrice = GetValueOrNull(values, 4),
                Elements = GetValueOrNull(values, 5),
                IsRetired = GetBoolValueOrFalse(values, 6)
            };
        }

        private async Task UpsertSets(IEnumerable<LegoSet> sets)
        {
            foreach (var set in sets)
            {
                await _dbContext.UpsertOneAsync(s => s.Number == set.Number, set);
            }
        }
    }
}
