using Autofac;
using CsvImporter.Import;
using Shared.Db;
using Shared.Helpers;

namespace CsvImporter.Helpers
{
    public static class ContainerProvider
    {
        public static IContainer Container { get; set; }

        static ContainerProvider()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ConfigurationReader>().As<IConfigurationReader>().SingleInstance();
            containerBuilder.RegisterType<LegoSetCsvImporter>().As<ILegoSetCsvImporter>().SingleInstance();
            //containerBuilder.RegisterType<DbContext<LegoSet>>().As<IDbContext<LegoSet>>().SingleInstance();
            containerBuilder.RegisterGeneric(typeof(DbContext<>)).As(typeof(IDbContext<>)).InstancePerDependency();

            Container = containerBuilder.Build();
        }
    }
}
