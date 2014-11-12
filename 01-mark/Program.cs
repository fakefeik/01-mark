using System.IO;

namespace _01_mark
{
    class Program
    {
        static string ReadFromFile(string filename)
        {
            string data;
            using (var reader = new StreamReader(filename))
            {
                data = reader.ReadToEnd();
            }
            return data;
        }

        static void WriteToFile(string filename, string data)
        {
            using (var writer = new StreamWriter(filename))
            {
                writer.Write(data);
            }
        }

        static void Main(string[] args)
        {
            var input = args[0];
            var output = args[1];
            var data = ReadFromFile(input);
            var processor = new MarkdownProccessor();
            var html = processor.ToHtml(data);
            WriteToFile(output, html);
        }
    }
}
