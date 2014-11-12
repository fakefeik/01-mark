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
            var processor = new MarkdownProccessor();
            Console.Write(processor.ToHtml(""));
            Assert.AreEqual(string.Format(Header, ""), processor.ToHtml(""));
        }

        [Test]
        public void Test_null()
        {
            var processor = new MarkdownProccessor();
            Assert.AreEqual(string.Format(Header, ""), processor.ToHtml(null));
        }

        [Test]
        public void Test_paragraph()
        {
            var processor = new MarkdownProccessor();
            var text = "sometext\n\nNew Parahraph\n\nsomeothertext";
            var textFormatted = "<p>sometext</p><p>New Parahraph</p><p>someothertext</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_paragraph_whitespaces()
        {
            var processor = new MarkdownProccessor();
            var text = "sometext  \n\r    \n\r\t   NewText  \n   \n other";
            var textFormatted = "<p>sometext  </p><p>\r\t   NewText  </p><p> other</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_simple_em_tags()
        {
            var processor = new MarkdownProccessor();
            var text = "some _statement other_ statement";
            var textFormatted = "<p>some <em>statement other</em> statement</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_em_tags_inside_words()
        {
            var processor = new MarkdownProccessor();
            var text = "Test_em_tags_inside_words";
            var textFormatted = "<p>Test_em_tags_inside_words</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_em_escape()
        {
            var processor = new MarkdownProccessor();
            var text = "ss \\_abc cde\\_ asd";
            var textFormatted = "<p>ss _abc cde_ asd</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_strong_tags()
        {
            var processor = new MarkdownProccessor();
            var text = "one __two three__ four";
            var textFormatted = "<p>one <strong>two three</strong> four</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_strong_escape()
        {
            var processor = new MarkdownProccessor();
            var text = "one \\__two three\\__ four";
            var textFormatted = "<p>one __two three__ four</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_strong_inside_em()
        {
            var processor = new MarkdownProccessor();
            var text = "Внутри _выделения em может быть __strong__ выделение_.";
            var textFormatted = "<p>Внутри <em>выделения em может быть <strong>strong</strong> выделение</em>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_em_inside_strong()
        {
            var processor = new MarkdownProccessor();
            var text = "Внутри __выделения strong может быть _em_ выделение__.";
            var textFormatted = "<p>Внутри <strong>выделения strong может быть <em>em</em> выделение</strong>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_code_tags()
        {
            var processor = new MarkdownProccessor();
            var text = "Текст окруженный `одинарными обратными кавычками`.";
            var textFormatted = "<p>Текст окруженный <code>одинарными обратными кавычками</code>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_code_tags_with_tags_inside()
        {
            var processor = new MarkdownProccessor();
            var text = "Текст окруженный `__одинарными__ _обратными_ кавычками`.";
            var textFormatted = "<p>Текст окруженный <code>__одинарными__ _обратными_ кавычками</code>.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_code_tags_escape()
        {
            var processor = new MarkdownProccessor();
            var text = "Текст окруженный \\`одинарными обратными кавычками\\`.";
            var textFormatted = "<p>Текст окруженный `одинарными обратными кавычками`.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_tags_within_words()
        {
            var processor = new MarkdownProccessor();
            var text = "Подчерки_внутри_текста__и__цифр_12_3";
            var textFormatted = "<p>Подчерки_внутри_текста__и__цифр_12_3</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_ordinary_tags()
        {
            var processor = new MarkdownProccessor();
            var text = "__непарные _символы не считаются `выделением.";
            var textFormatted = "<p>__непарные _символы не считаются `выделением.</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }

        [Test]
        public void Test_escape_characters_replace()
        {
            var processor = new MarkdownProccessor();
            var text = "one <p>two three</p> four";
            var textFormatted = "<p>one &ltp&gttwo three&lt/p&gt four</p>";
            Assert.AreEqual(string.Format(Header, textFormatted), processor.ToHtml(text));
        }
    }
}
