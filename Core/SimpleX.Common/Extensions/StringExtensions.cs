using System.Text.RegularExpressions;

namespace SimpleX.Common.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveDoubleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ").Trim();
        }

        public static bool IsNumeric(this string value)
        {
            return decimal.TryParse(value, out _);
        }

        public static decimal ToDecimal(this string value)
        {
            return decimal.Parse(value);
        }

        public static string IfEmptyThen(this string value, string valueIfEmpty)
        {
            if (string.IsNullOrWhiteSpace(value))
                return valueIfEmpty;
            else
                return value;
        }

        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length <= length)
                return value;

            return value.Substring(0, length);
        }

        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length <= length)
                return value;

            return value.Substring(value.Length - length, length);
        }
    }
}
