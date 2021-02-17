using CommandLine;

namespace CsvImporter.Helpers
{
    public class CmdOptions
    {
        [Option('o', "ownerships", Required = false, HelpText = "Input files with set ownerships.")]
        public string OwnershipsFile { get; set; }

        [Option('s', "sets", Required = false, HelpText = "Input files with sets.")]
        public string SetsFile { get; set; }
    }
}
