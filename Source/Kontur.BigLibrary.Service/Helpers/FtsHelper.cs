using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kontur.BigLibrary.Service.Helpers
{
public static class FtsHelper
    {
        //https://www.postgresql.org/docs/9.6/textsearch-limitations.html
        private static readonly int lexemMaxLength = 2046 / 2;

        // Разделители
        // 1. безусловные разделители
        private static readonly string delimiters1 = @"\s|[:\|«»&!@#$%^*();+-/\\""=\[\]\<\>?]";

        // 2. '.' и '''' - разделители, если не располагаются непосредственно между буквами или цифрами
        private static readonly string delimiters2 = @"((?<=\D)[\.'](?=\d))|((?<=\d)[\.'](?=\D))|((?<=\P{L})[\.'](?=\p{L}))|((?<=\p{L})[\.'](?=\P{L}))|((?<=\W)[\.'])|([\.'](?=\W))";

        // 3. ',' и '`' - разделители, если не располагаются непосредственно между цифрами
        private static readonly string delimiters3 = @"((?<=\D)[,`](?=\d))|((?<=\d)[,`](?=\D))|((?<=\D)[,`](?=\D))";

        // Итого - все возможные разделители
        private static readonly string delimiters = $@"{delimiters1}|{delimiters2}|{delimiters3}";


        // Не разделители (приходится явно указывать возможность их нахождения внутри токенов)
        // 1. '.' и '''' - не разделители между цифрами и буквами
        private static readonly string nonDelimiters1 = @"((?<=\d)[\.'](?=\d))|((?<=\p{L})[\.'](?=\p{L}))";

        // 2. ',' и '`' - не разделители, если располгаются непосредственно между цифрами
        private static readonly string nonDelimiters2 = @"((?<=\d)[,`](?=\d))";

        // Итого - все возможные неразделители
        private static readonly string nonDelimiters = $@"{nonDelimiters1}|{nonDelimiters2}";

        // Замены символов
        // 1. '.'. ',', '''', и '`' - между цифр заменяем на ','
        private static readonly string replace1 = @"(?<=\d)[\.'`](?=\d)";
        private static readonly string replacement1 = ",";


        private static readonly Regex tokenRegex = new Regex($@"(\w|{nonDelimiters})+(?={delimiters}|$)", RegexOptions.Compiled);
        private static readonly Regex replaceRegex1 = new Regex(replace1, RegexOptions.Compiled);

        public static string GetLexemsWithPositions(string source)
        {
            var lexems = GetLexems(source);

            var result = string.Empty;

            for (int i = 0; i < lexems.Length; i++)
            {
                if (lexems[i].Length >= lexemMaxLength)
                {
                    lexems[i] = lexems[i].Substring(0, lexemMaxLength);
                }

                if (result != string.Empty)
                {
                    result += " ";
                }

                result += EscapeQuoteSymbols(lexems[i]);
            }

            return result;
        }

        public static string[] GetLexems(string source)
        {
            if (source == null)
            {
                return Array.Empty<string>();
            }

            var result = new List<string>();
            var match = tokenRegex.Match(source);

            while (match.Success)
            {
                var token = replaceRegex1.Replace(match.Value, replacement1).ToLower();
                result.Add(token);

                match = match.NextMatch();
            }

            return result.ToArray();
        }

        public static string GetPrefixQuery(string source)
        {
            if (source == null)
            {
                return null;
            }

            var lexems = GetLexems(source);

            if (lexems.Length == 0)
            {
                return string.Empty;
            }

            return lexems.Select(EscapeQuoteSymbols).Aggregate((l1, l2) => $"{l1} OR {l2}");
        }

        private static string EscapeQuoteSymbols(string lexem)
        {
            return lexem.Replace("'", "''");
        }
    }
}