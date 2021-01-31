using Shared.Helpers;
using System.Configuration;

namespace CsvImporter.Helpers
{
    public class ConfigurationReader : IConfigurationReader
    {
        public string GetConnectionString() =>
            ConfigurationManager.AppSettings["ConnectionString"];

        public string GetDatabaseName() =>
            ConfigurationManager.AppSettings["DatabaseName"];
    }
}
