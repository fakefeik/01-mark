﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_mark
{
    class HtmlTags
    {
        public const string Open = "<{0}>";
        public const string Close = "</{0}>";
        public const string Header = @"<!DOCTYPE html>
<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
</head>
<body>
{0}
</body>
</html>";
        public static readonly Dictionary<string, string> Tags = new Dictionary<string, string> 
        { 
            { "_", "em" },
            { "__", "strong" }, 
            { "`", "code" } 
        };
        public static readonly Dictionary<string, string> Escape = new Dictionary<string, string>
        {
            { "<", "&lt" },
            { ">", "&gt" }
        }; 
    }
}
