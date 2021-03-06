﻿using Shared.Helpers;
using System;

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
