using Shared.Helpers;

namespace CsvImporter.Import
{
    public class BaseCsvImporter
    {
        internal static string GetValueOrNull(string[] values, int index) =>
            values.Length >= index
                ? values[index]
                : null;

        internal static bool GetBoolValueOrFalse(string[] values, int index) =>
            values.Length <= index
                ? false
                : ParseHelper.GetBoolValueOrFalse(values[index]);

        internal static int GetIntValueOrZero(string[] values, int index) =>
            values.Length <= index
                ? 0
                : ParseHelper.GetIntValueOrFalse(values[index]);
    }
}
