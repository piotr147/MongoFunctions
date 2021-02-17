namespace CsvImporter.Import
{
    public class BaseCsvImporter
    {
        internal static string GetValueOrNull(string[] values, int index) =>
            values.Length >= index
                ? values[index]
                : null;

        internal static bool GetBoolValueOrFalse(string[] values, int index)
        {
            if (values.Length <= index)
            {
                return false;
            }

            if (int.TryParse(values[index], out int resultInt))
            {
                return resultInt != 0;
            }

            if (!bool.TryParse(values[index], out bool result))
            {
                return false;
            }

            return result;
        }

        internal static int GetIntValueOrZero(string[] values, int index)
        {
            if (values.Length <= index || !int.TryParse(values[index], out int result))
            {
                return 0;
            }

            return result;
        }
    }
}
