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
            Initialize();
            ReadArgs(args, out string setsFile, out string ownershipsFile);

            try
            {
                await TryImport(setsFile, ownershipsFile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            using ILifetimeScope scope = ContainerProvider.Container.BeginLifetimeScope();
            _legoSetCsvImporter = ContainerProvider.Container.Resolve<ILegoSetCsvImporter>();
            _setOwnershipCsvImporter = ContainerProvider.Container.Resolve<ISetOwnershipCsvImporter>();
        }

        private static void ReadArgs(string[] args, out string setsFile, out string ownershipsFile)
        {
            string s1 = string.Empty, s2 = string.Empty;

            Parser.Default.ParseArguments<CmdOptions>(args)
                   .WithParsed(o =>
                   {
                       s1 = o.SetsFile;
                       s2 = o.OwnershipsFile;
                   });

            setsFile = s1;
            ownershipsFile = s2;
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

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject.ToString());
            Environment.Exit(1);
        }
    }
}
