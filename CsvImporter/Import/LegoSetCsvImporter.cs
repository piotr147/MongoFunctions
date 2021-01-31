using Shared.Db;
using Shared.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvImporter.Import
{
    public class LegoSetCsvImporter : ILegoSetCsvImporter
    {
        private IDbContext<LegoSet> _dbContext;

        public LegoSetCsvImporter(IDbContext<LegoSet> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ImportSets(string path)
        {
            IEnumerable<LegoSet> sets = await ReadSets(path);
            await _dbContext.InsertManyAsync(sets);
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
                Elements = GetValueOrNull(values, 5)
            };
        }

        private static string GetValueOrNull(string[] values, int index) =>
            values.Length > index
                ? values[index]
                : null;
    }
}
