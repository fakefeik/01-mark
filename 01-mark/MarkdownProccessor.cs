using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _01_mark
{
    class MarkdownProccessor : IMarkdownProcessor
    {
        private static string AddHeader(string text)
        {
            return string.Format(HtmlTags.Header, text);
        }

        private string Reformat(string text, Stack<Tuple<string, int>> strings)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '`')
                {
                    text = ReplaceMarkTags(text, "`", i, strings);
                }
                if (text[i] == '_' && text[i + 1] == '_')
                {
                    if (strings.Count != 0 && strings.Peek().Item1 == "`")
                        continue;
                    text = ReplaceMarkTags(text, "__", i, strings);
                }
                else if (text[i] == '_')
                {
                    if (strings.Count != 0 && strings.Peek().Item1 == "`")
                        continue;
                    text = ReplaceMarkTags(text, "_", i, strings);
                }
            }
            return text;
        }

        private string ReplaceMarkTags(string text, string tag, int i, Stack<Tuple<string, int>> strings)
        {
            if ((strings.Count == 0 || strings.Peek().Item1 != tag) && (i == 0 || IsNotInWord(text[i - 1])))
                strings.Push(new Tuple<string, int>(tag, i));
            else if (strings.Count != 0 && strings.Peek().Item1 == tag && (i == text.Length - tag.Length || IsNotInWord(text[i + tag.Length])))
            {
                var prev = strings.Pop().Item2;
                text = InsertTags(text, HtmlTags.Tags[tag], prev, i, tag.Length);
            }
            return text;
        }

        private static bool IsNotInWord(char c)
        {
            return !char.IsLetterOrDigit(c) && c != '\\' && c != '_';
        }

        private static string InsertTags(string text, string tag, int from, int to, int toDelete)
        {
            text = text.Remove(to, toDelete);
            text = text.Insert(to, string.Format(HtmlTags.Close, tag));
            text = text.Remove(from, toDelete);
            text = text.Insert(from, string.Format(HtmlTags.Open, tag));
            return text;
        }

        private static string ReplaceHtmlEscapeCharacters(string text)
        {
            return HtmlTags.Escape.Keys.Aggregate(text, (current, character) => Regex.Replace(current, character, HtmlTags.Escape[character]));
        }

        private string AddParagraphs(string text, Stack<Tuple<string, int>> strings)
        {
            return String.Join("", Regex.Split(text, @"\n\r*\s*\n")
                .Select(ReplaceHtmlEscapeCharacters)
                .Select(x => string.Format("<p>{0}</p>", x))
                .Select(x => Reformat(x, strings))
                .Select(x => Regex.Replace(x , @"\\", "")));
        }

        public string ToHtml(string text)
        {
            var strings = new Stack<Tuple<string, int>>();
            if (string.IsNullOrEmpty(text))
                return AddHeader("");
            return AddHeader(AddParagraphs(text, strings));
        }
    }
}
