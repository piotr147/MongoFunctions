using System;
using System.Configuration;

namespace MongoFunctions.Helpers
{
    public class ConfigurationReader : IConfigurationReader
    {
        public string GetConnectionString() =>
            Environment.GetEnvironmentVariable("ConnectionString");
            

        public string GetDatabaseName() =>
            Environment.GetEnvironmentVariable("DatabaseName");
    }
}
