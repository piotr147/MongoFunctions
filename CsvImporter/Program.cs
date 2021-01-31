using Autofac;
using CsvImporter.Helpers;
using CsvImporter.Import;
using System;
using System.Threading.Tasks;

namespace CsvImporter
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string path = GetPathFromArgs(args);

            using ILifetimeScope scope = ContainerProvider.Container.BeginLifetimeScope();

            ILegoSetCsvImporter legoSetCsvImporter = ContainerProvider.Container.Resolve<ILegoSetCsvImporter>();


            try
            {
                await legoSetCsvImporter.ImportSets(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string GetPathFromArgs(string[] args) =>
            args[0];
    }
}
