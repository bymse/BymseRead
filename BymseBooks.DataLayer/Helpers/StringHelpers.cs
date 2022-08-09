using System.Text.RegularExpressions;

namespace BymseBooks.DataLayer.Helpers
{
    public static class StringHelpers
    {
        public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);

        public static string[] SplitTrim(this string str, string separator)
            => str
                .Split(separator)
                .Select(e => e.Trim())
                .ToArray();

        public static IDictionary<string, string> FromCookiesString(this string cookies) => cookies
            .SplitTrim(";")
            .Where(e => !e.IsNullOrEmpty())
            .Select(e =>
            {
                var split = e.Split('=');
                return new
                {
                    key = split[0],
                    value = split[1]
                };
            })
            .ToDictionary(e => e.key, e => e.value);

        public static string? ExtractWord(this string wordsStr, int wordIndex, string separator)
        {
            var words = wordsStr.SplitTrim(separator);
            return words.Length - 1 < wordIndex
                ? null
                : words[wordIndex];
        }

        public static string JoinStrings(this IEnumerable<string> strings, string separator = "")
        {
            return string.Join(separator, strings);
        }

        public static string JoinNotNullOrEmpty(this IEnumerable<string> strings, string separator = "")
        {
            return strings.Where(e => !e.IsNullOrEmpty()).JoinStrings(separator);
        }

        public static string StartWithHex(this string str) => $"#{str}";

        public static string Replace(this string str, Regex regex, string replaceVal) => regex.Replace(str, replaceVal);

        public static string FallbackWith(this string src, string fallback) => src.IsNullOrEmpty() ? fallback : src;
        public static string IfNotNullOrEmpty(this string src, Func<string, string> @do, Func<string, string>? @else = null)
        {
            return src.IsNullOrEmpty() ? (@else ?? (s => s))(src) : @do(src);
        }
    }
}