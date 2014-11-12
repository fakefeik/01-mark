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
        private static Stack<int> strings = new Stack<int>();
        private static string open = "<{0}>";
        private static string close = "</{0}>";
        private static Dictionary<string, string> tags = new Dictionary<string, string> 
        { 
            { "_", "em"}, 
            { "__", "strong" }, 
            { "`", "code" } 
        }; 
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

        //TODO
        private static string Reformat(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '_')
                {
                    if (strings.Count == 0 && (i == 0 || text[i - 1] == ' '))
                        strings.Push(i);
                    else if (strings.Count != 0 && (i == text.Length - 1 || text[i + 1] == ' '))
                    {
                        var prev = strings.Pop();
                        text = text.Remove(i, 1);
                        text = text.Remove(prev, 1);
                        text = text.Insert(i - 1, "</em>");
                        text = text.Insert(prev, "<em>");
                    }
                }
            }
            text = Regex.Replace(text, @"\\", "");
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
                .Select(Reformat));
        }

        public string ToHtml(string text)
        {
            strings = new Stack<int>();
            if (string.IsNullOrEmpty(text))
                return AddHeader("");
            return AddHeader(AddParagraphs(text));
        }
    }
}
