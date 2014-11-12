using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace _01_mark
{
    class MarkdownProccessor : IMarkdownProcessor
    {
        /*private static Dictionary<string, string> tags = new Dictionary<string, string> 
        { 
            { "_", "em"}, 
            { "__", "strong" }, 
            { "`", "code" } 
        }; */

        private static string open = "<{0}>";
        private static string close = "</{0}>";
        private static Stack<Tuple<string, int>> strings = new Stack<Tuple<string, int>>(); 
        private static Dictionary<string, string> escape = new Dictionary<string, string>
        {
            //TODO: add some other stuff
            { "<", "&lt" },
            { ">", "&gt" }
        }; 


        private static string AddHeader(string text)
        {
            const string header = @"<!DOCTYPE html>
<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
</head>
<body>
{0}
</body>
</html>";
            return string.Format(header, text);
        }

        //TODO: refactor this
        private static string Reformat(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '`')
                {
                    if ((strings.Count == 0 || strings.Peek().Item1 != "`") && (i == 0 || IsNotInWord(text[i - 1])))
                        strings.Push(new Tuple<string, int>("`", i));
                    else if (strings.Count != 0 && strings.Peek().Item1 == "`" && (i == text.Length - 1 || IsNotInWord(text[i + 1])))
                    {
                        var prev = strings.Pop().Item2;
                        text = InsertTags(text, "code", prev, i, 1);
                    }
                }
                if (text[i] == '_' && text[i + 1] == '_')
                {
                    if (strings.Count != 0 && strings.Peek().Item1 == "`")
                        continue;
                    if ((strings.Count == 0 || strings.Peek().Item1 != "__") && (i == 0 || IsNotInWord(text[i - 1])))
                        strings.Push(new Tuple<string, int>("__", i));
                    else if (strings.Count != 0 && strings.Peek().Item1 == "__" && (i == text.Length - 2 || IsNotInWord(text[i + 2])))
                    {
                        var prev = strings.Pop().Item2;
                        text = InsertTags(text, "strong", prev, i, 2);
                    }
                }
                else if (text[i] == '_')
                {
                    if (strings.Count != 0 && strings.Peek().Item1 == "`")
                        continue;
                    if ((strings.Count == 0 || strings.Peek().Item1 != "_") && (i == 0 || IsNotInWord(text[i - 1])))
                        strings.Push(new Tuple<string, int>("_", i));
                    else if (strings.Count != 0 && strings.Peek().Item1 == "_" && (i == text.Length - 1 || IsNotInWord(text[i + 1])))
                    {
                        var prev = strings.Pop().Item2;
                        text = InsertTags(text, "em", prev, i, 1);
                    }
                }
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
            text = text.Insert(to, string.Format(close, tag));
            text = text.Remove(from, toDelete);
            text = text.Insert(from, string.Format(open, tag));
            return text;
        }

        private static string ReplaceHtmlEscapeCharacters(string text)
        {
            foreach (var character in escape.Keys)
            {
                text = Regex.Replace(text, character, escape[character]);
            }
            return text;
        }

        private static string AddParagraphs(string text)
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
