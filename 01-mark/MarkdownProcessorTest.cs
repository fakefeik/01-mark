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
    internal class MarkdownProcessorTest
    {
        private const string header = @"<!DOCTYPE html>
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

        [Test]
        public void Test_paragraph_whitespaces()
        {
            var text = "sometext  \n\r    \n\r\t   NewText  \n   \n other";
            var textFormatted = "<p>sometext  </p><p>\r\t   NewText  </p><p> other</p>";
            Assert.AreEqual(string.Format(header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_simple_em_tags()
        {
            var text = "some _statement other_ statement";
            var textFormatted = "<p>some <em>statement other</em> statement</p>";
            Assert.AreEqual(string.Format(header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_em_tags_inside_words()
        {
            var text = "Test_em_tags_inside_words";
            var textFormatted = "<p>Test_em_tags_inside_words</p>";
            Assert.AreEqual(string.Format(header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_ref()
        {
            var text = "ss \\_abc cde\\_ asd";
            var textFormatted = "<p>ss _abc cde_ asd</p>";
            Assert.AreEqual(string.Format(header, textFormatted), MarkdownProccessor.ToHtml(text));
        }
    }
}
