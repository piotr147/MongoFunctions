using System;

namespace Shared.Helpers
{
    public static class ParseHelper
    {
        public static bool GetBoolValueOrFalse(string value)
        {
            if (int.TryParse(value, out int resultInt))
            {
                return resultInt != 0;
            }

            if (!bool.TryParse(value, out bool result))
            {
                return false;
            }

            return result;
        }

        public static int GetIntValueOrFalse(string value) =>
            int.TryParse(value, out int result)
                ? result
                : 0;

        public static DateTime GetDateTimeValueOrMin(string value) =>
            DateTime.TryParse(value, out DateTime result)
                ? result
                : DateTime.MinValue;
    }
}
