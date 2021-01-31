using System.Threading.Tasks;

namespace CsvImporter.Import
{
    public interface ILegoSetCsvImporter
    {
        public Task ImportSets(string path);
    }
}
