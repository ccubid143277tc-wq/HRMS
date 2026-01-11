using System;
using System.Globalization;

namespace HRMS.Helper
{
    public static class MoneyHelper
    {
        public static decimal Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0m;
            }

            string cleaned = value.Replace("₱", "").Replace(",", "").Trim();

            if (decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsed))
            {
                return parsed;
            }

            if (decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
            {
                return parsed;
            }

            return 0m;
        }

        public static string Format(decimal amount)
        {
            return $"₱{amount:0.00}";
        }

        public static string FormatWithSpace(decimal amount)
        {
            return $"₱ {amount:0.00}";
        }
    }
}
