using System;
using NUnit.Framework;

namespace _01_mark
{
    [TestFixture]
    internal class MarkdownProcessorTest
    {
        private const string Header = @"<!DOCTYPE html>
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
            Assert.AreEqual(string.Format(Header, ""), MarkdownProccessor.ToHtml(""));
        }

        [Test]
        public void Test_null()
        {
            Assert.AreEqual(string.Format(Header, ""), MarkdownProccessor.ToHtml(null));
        }

        [Test]
        public void Test_paragraph()
        {
            var text = "sometext\n\nNew Parahraph\n\nsomeothertext";
            var textFormatted = "<p>sometext</p><p>New Parahraph</p><p>someothertext</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_paragraph_whitespaces()
        {
            var text = "sometext  \n\r    \n\r\t   NewText  \n   \n other";
            var textFormatted = "<p>sometext  </p><p>\r\t   NewText  </p><p> other</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_simple_em_tags()
        {
            var text = "some _statement other_ statement";
            var textFormatted = "<p>some <em>statement other</em> statement</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_em_tags_inside_words()
        {
            var text = "Test_em_tags_inside_words";
            var textFormatted = "<p>Test_em_tags_inside_words</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_em_escape()
        {
            var text = "ss \\_abc cde\\_ asd";
            var textFormatted = "<p>ss _abc cde_ asd</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_strong_tags()
        {
            var text = "one __two three__ four";
            var textFormatted = "<p>one <strong>two three</strong> four</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_strong_escape()
        {
            var text = "one \\__two three\\__ four";
            var textFormatted = "<p>one __two three__ four</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_strong_inside_em()
        {
            var text = "Внутри _выделения em может быть __strong__ выделение_.";
            var textFormatted = "<p>Внутри <em>выделения em может быть <strong>strong</strong> выделение</em>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_em_inside_strong()
        {
            var text = "Внутри __выделения strong может быть _em_ выделение__.";
            var textFormatted = "<p>Внутри <strong>выделения strong может быть <em>em</em> выделение</strong>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_code_tags()
        {
            var text = "Текст окруженный `одинарными обратными кавычками`.";
            var textFormatted = "<p>Текст окруженный <code>одинарными обратными кавычками</code>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_code_tags_with_tags_inside()
        {
            var text = "Текст окруженный `__одинарными__ _обратными_ кавычками`.";
            var textFormatted = "<p>Текст окруженный <code>__одинарными__ _обратными_ кавычками</code>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_code_tags_escape()
        {
            var text = "Текст окруженный \\`одинарными обратными кавычками\\`.";
            var textFormatted = "<p>Текст окруженный `одинарными обратными кавычками`.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_tags_within_words()
        {
            var text = "Подчерки_внутри_текста__и__цифр_12_3";
            var textFormatted = "<p>Подчерки_внутри_текста__и__цифр_12_3</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_ordinary_tags()
        {

            var text = "__непарные _символы не считаются `выделением.";
            var textFormatted = "<p>__непарные _символы не считаются `выделением.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }

        [Test]
        public void Test_escape_characters_replace()
        {
            var text = "one <p>two three</p> four";
            var textFormatted = "<p>one &ltp&gttwo three&lt/p&gt four</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), MarkdownProccessor.ToHtml(text));
        }
    }
}
