using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _01_mark
{
    class MarkdownProccessor
    {
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

        public static string AddParagraphs(string text)
        {
            return string.Format("<p>{0}</p>", text);
        }

        public static string ToHtml(string text)
        {
            return AddHeader(text);
            
        }

    }
}
