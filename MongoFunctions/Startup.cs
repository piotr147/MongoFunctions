using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoFunctions.Helpers;
using Shared.Db;
using Shared.Helpers;
using Shared.Models;

[assembly: FunctionsStartup(typeof(MongoFunctions.Startup))]

namespace MongoFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IConfigurationReader>(new ConfigurationReader());
            builder.Services.AddScoped<IDbContext<LegoSet>, DbContext<LegoSet>>();
            builder.Services.AddScoped<IDbContext<SetOwnership>, DbContext<SetOwnership>>();
        }
    }
}
