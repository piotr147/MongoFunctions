using Autofac;
using CommandLine;
using CsvImporter.Helpers;
using CsvImporter.Import;
using System;
using System.Threading.Tasks;

namespace CsvImporter
{
    class Program
    {
        private static ILegoSetCsvImporter _legoSetCsvImporter;
        private static ISetOwnershipCsvImporter _setOwnershipCsvImporter;

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            using ILifetimeScope scope = ContainerProvider.Container.BeginLifetimeScope();
            _legoSetCsvImporter = ContainerProvider.Container.Resolve<ILegoSetCsvImporter>();
            _setOwnershipCsvImporter = ContainerProvider.Container.Resolve<ISetOwnershipCsvImporter>();

            string setsFile = string.Empty, ownershipsFile = string.Empty;

            Parser.Default.ParseArguments<CmdOptions>(args)
                   .WithParsed(o =>
                   {
                       setsFile = o.SetsFile;
                       ownershipsFile = o.OwnershipsFile;
                   });

            try
            {
                await TryImport(setsFile, ownershipsFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task TryImport(string setsFile, string ownershipsFile)
        {
            if(!string.IsNullOrWhiteSpace(setsFile))
            {
                await _legoSetCsvImporter.ImportSets(setsFile);
            }

            if (!string.IsNullOrWhiteSpace(ownershipsFile))
            {
                await _setOwnershipCsvImporter.ImportSets(ownershipsFile);
            }
        }

        private static string GetPathFromArgs(string[] args) =>
            args[0];

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
    }
}
