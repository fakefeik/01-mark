using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _01_mark
{
    class MarkdownProccessor : IMarkdownProcessor
    {
        private static readonly Dictionary<string, string> tags = new Dictionary<string, string> 
        { 
            { "_", "em" },
            { "__", "strong" }, 
            { "`", "code" } 
        };
        private static readonly Dictionary<string, string> escape = new Dictionary<string, string>
        {
            { "<", "&lt" },
            { ">", "&gt" }
        }; 

        private Stack<Tuple<string, int>> strings = new Stack<Tuple<string, int>>(); 

        private static string AddHeader(string text)
        {
            return string.Format(HtmlTags.Header, text);
        }

        private string Reformat(string text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '`')
                {
                    text = ReplaceMarkTags(text, "`", i);
                }
                if (text[i] == '_' && text[i + 1] == '_')
                {
                    if (strings.Count != 0 && strings.Peek().Item1 == "`")
                        continue;
                    text = ReplaceMarkTags(text, "__", i);
                }
                else if (text[i] == '_')
                {
                    if (strings.Count != 0 && strings.Peek().Item1 == "`")
                        continue;
                    text = ReplaceMarkTags(text, "_", i);
                }
            }
            return text;
        }

        private string ReplaceMarkTags(string text, string tag, int i)
        {
            if ((strings.Count == 0 || strings.Peek().Item1 != tag) && (i == 0 || IsNotInWord(text[i - 1])))
                strings.Push(new Tuple<string, int>(tag, i));
            else if (strings.Count != 0 && strings.Peek().Item1 == tag && (i == text.Length - tag.Length || IsNotInWord(text[i + tag.Length])))
            {
                var prev = strings.Pop().Item2;
                text = InsertTags(text, tags[tag], prev, i, tag.Length);
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
            return escape.Keys.Aggregate(text, (current, character) => Regex.Replace(current, character, escape[character]));
        }

        private string AddParagraphs(string text)
        {
            return String.Join("", Regex.Split(text, @"\n\r*\s*\n")
                .Select(ReplaceHtmlEscapeCharacters)
                .Select(x => string.Format("<p>{0}</p>", x))
                .Select(Reformat)
                .Select(x => Regex.Replace(x , @"\\", "")));
        }

        public string ToHtml(string text)
        {
            strings = new Stack<Tuple<string, int>>();
            if (string.IsNullOrEmpty(text))
                return AddHeader("");
            return AddHeader(AddParagraphs(text));
        }
    }
}
