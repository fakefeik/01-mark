using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace _01_mark
{
    [TestFixture]
    class MarkdownProcessorTest
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

        [Test]
        public void Test_empty()
        {
            Console.Write(MarkdownProccessor.ToHtml(""));
            Assert.AreEqual(string.Format(header, ""), MarkdownProccessor.ToHtml(""));
        }

        [Test]
        public void Test_null()
        {
            Assert.AreEqual(string.Format(header, ""), MarkdownProccessor.ToHtml(null));
        }

        [Test]
        public void Test_paragraph()
        {
            var text = "sometext\n\nNew Parahraph\n\nsomeothertext";
            var textFormatted = "<p>sometext</p><p>New Parahraph</p><p>someothertext</p>";
            Assert.AreEqual(string.Format(header, textFormatted), MarkdownProccessor.ToHtml(text));
        }
    }
}
