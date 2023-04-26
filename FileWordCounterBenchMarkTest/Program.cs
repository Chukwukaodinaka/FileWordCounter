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
        public string filePath = @"C:\Users\esc\OneDrive\Documents\txtdocs\sample-2mb-text-file.txt";

        [Benchmark]
        public List<string> GetEachWordInContentUsingRegex()
        {
            return Regex.Matches(File.ReadAllText(filePath), @"\w+").Cast<Match>()
                  .Select(x => x.Value)
                  .ToList();
        }

        [Benchmark]
        public  List<string> GetEachWordInContentUsingStringReader()
        {
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            StringReader reader = new(File.ReadAllText(filePath));
            return reader.ReadToEnd().Split(delimiterChars).ToList();
        }
    }
}