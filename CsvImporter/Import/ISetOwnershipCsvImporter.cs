using System.Threading.Tasks;

namespace CsvImporter.Import
{
    public interface ISetOwnershipCsvImporter
    {
        public Task ImportSets(string path);
    }
}
