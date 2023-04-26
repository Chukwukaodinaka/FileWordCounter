using FileWordCounter;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<FileRead>();
    }

    public class FileRead
    {
        public string filePath = @"C:\Users\esc\OneDrive\Documents\txtdocs\sample1.txt";

        [Benchmark]
        public List<string> GetEachWordInContentUsingReadAsText()
        {
            //return Regex.Matches(File.ReadAllText(filePath), @"\w+").Cast<Match>()
            //      .Select(x => x.Value)
            //      .ToList();

            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            return File.ReadAllText(filePath).Split(delimiterChars).ToList();
        }

        [Benchmark]
        public  List<string> GetEachWordInContentUsingReadToEnd()
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            StringReader reader = new(filePath);
            return reader.ReadToEnd().Split(delimiterChars).ToList();
        }
    }
}