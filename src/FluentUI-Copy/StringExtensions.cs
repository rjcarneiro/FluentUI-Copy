using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FluentUI.System.Icons.Copier.App
{
    public static class StringExtensions
    {
        public static ICollection<string> SplitCamelCase(this string source)
        {
            const string pattern = @"[A-Z][a-z]*|[a-z]+|\d+";
            var matches = Regex.Matches(source, pattern);

            return matches.Select(t => t.Value).ToList();
        }

        public static string FirstCharToUpper(this string input) => input.First().ToString().ToUpper() + input.Substring(1);
    }
}