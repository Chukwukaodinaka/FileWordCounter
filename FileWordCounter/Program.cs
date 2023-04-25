using FileWordCounter;
using System.Text.RegularExpressions;
using System.Xml.Linq;

class Program
{
    private static WordOccurenceDictionary wordOccurenceDictionary;
    private static string inputDirectory;

    public static async Task Main(string[] args)
    {
        inputDirectory = GetInputFromUser();
        var fileData = await GetDataFromFilesInDirectory();
        wordOccurenceDictionary = CountOccurrenceForEachWordFromList(fileData);
        var excludedWordDictionary = ExcludeWordsFoundInExcludeFile();
        await GenerateFileForExludedWords(excludedWordDictionary);
        await GenerateFileForEachLetterInTheAplhabet();
    }

    private static async Task<string[]> GetDataFromFilesInDirectory()
    {
        return await FileHandler.GetDataFromFilesInDirectory(inputDirectory);
    }

    private static WordOccurenceDictionary CountOccurrenceForEachWordFromList(string[] fileData)
    {
       return WordOccurrenceCounter.CountOccurrenceForEachWordFromList(fileData);
    }
    private static Dictionary<string,int> ExcludeWordsFoundInExcludeFile()
    {
        var excludePath = $"{inputDirectory}/exclude.txt";
        if (!File.Exists(excludePath))
        {
            Console.WriteLine("exclude.txt does not exist!");
            return default;
        }
        var excludeFileContent = FileHandler.GetTextsFromFile(excludePath);
        var excludeWords = WordOccurrenceCounter.GetEachWordInContent(excludeFileContent);
       return  wordOccurenceDictionary.ExcludeWordsFromDictionary(excludeWords);
    }

    private static string? GetInputFromUser()
    {
        Console.WriteLine("Please input specific directory");
        var inputDirectory = Console.ReadLine();
        return inputDirectory;
    }

    private static Task GenerateFileForExludedWords(Dictionary<string,int> excludedDictionary)
    {
        return Task.Run(() =>
        {
            var fileName = $"{inputDirectory}/excludedResult.txt";
            var fileContent = FileHandler.GenerateFileContentUsingDictionary(excludedDictionary);
            FileHandler.CreateFile(fileName, fileContent);
        });
    }

    private static Task GenerateFileForEachLetterInTheAplhabet()
    {
        var wordDictionary = wordOccurenceDictionary.wordOccurrence;
        var tasks = new List<Task>();

        for (char value = 'A'; value <= 'Z'; value++)
        {
            var letter = value;
            tasks.Add(Task.Run(() =>
            {
                var fileName = $"{inputDirectory}/{letter}.txt";
                var fileAppendDictionary = wordOccurenceDictionary.GetDictionaryOfWordsStartingWith(letter);
                var fileContent = FileHandler.GenerateFileContentUsingDictionary(fileAppendDictionary);
                FileHandler.CreateFile(fileName, fileContent);
            }));
        }
        return Task.WhenAll(tasks);
    }
}

