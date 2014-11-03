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
            const string empty = @"<!DOCTYPE html>
<html>
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
</head>
<body>

</body>
</html>";
            Console.Write(MarkdownProccessor.ToHtml(""));
            Assert.AreEqual(empty, MarkdownProccessor.ToHtml(""));
        }

        [Test]
        public void Test_paragraph()
        {
            var text = "sometext\n\nNew Parahraph\n\nsomeothertext";
            var textFormatted = "sometext<p>NewParahraph</p>someothertext";
            Assert.AreEqual(string.Format(header, textFormatted), MarkdownProccessor.ToHtml(text));
        }
    }
}
