using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoFunctions.Db;
using MongoFunctions.Helpers;
using MongoFunctions.Models;

[assembly: FunctionsStartup(typeof(MongoFunctions.Startup))]

namespace MongoFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();

            //builder.Services.AddSingleton<IMyService>((s) => {
            //    return new MyService();
            //});

            builder.Services.AddSingleton<IConfigurationReader>(new ConfigurationReader());
            builder.Services.AddScoped<IDbContext<LegoSet>, DbContext<LegoSet>>();
        }
    }
}
