using System.Text.RegularExpressions;
using System.Xml.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        var fileWordCounter = new FileWordCounter();
        await fileWordCounter.Start();
    }
}

class WordCount
{
    public string Word { get; set; }
    public int Count { get; set; }
}

class FileWordCounter
{
    Dictionary<string, int> wordOccurrence = new();
    Dictionary<string, int> excludedWordOccurence = new();
    private string? inputDirectory;


    public async Task Start()
    {
        inputDirectory = GetInputFromUser();
        await UpdateDictionaryForEachFileInDirectory();
        ExcludeWordsFoundInExcludeFile();
        await GenerateFileForExludedWords();
        await GenerateFileForEachLetterInTheAplhabet();
    }

    private string? GetInputFromUser()
    {
        Console.WriteLine("Please input specific directory");
        var inputDirectory = Console.ReadLine();
        return inputDirectory;
    }



    private async Task UpdateDictionaryForEachFileInDirectory()
    {
        if (!Directory.Exists(inputDirectory))
        {
            return;
        }
        var filePathsOfDirectory = Directory.GetFiles(inputDirectory);
        await UpdateDictionaryFromFiles(filePathsOfDirectory);
    }

    private Task UpdateDictionaryFromFiles(string[] filePathsOfDirectory)
    {
        var tasks = new List<Task>();

        foreach (var filePath in filePathsOfDirectory)
        {
            Console.WriteLine($"Processing file {filePath}");
            tasks.Add(Task.Run(() =>
            {
                UpdateDictionaryFromFile(filePath);
            }));
        }

        return Task.WhenAll(tasks);

    }

    private void UpdateDictionaryFromFile(string filePath)
    {
        if (FileExistsAndIsNotExclude(filePath))
        {
            var wordsList = GetListOfWordCountForFile(filePath);
            UpdateDictionaryWithList(wordsList);
        }
    }

    private bool FileExistsAndIsNotExclude(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return false;
        }

        if (filePath.EndsWith("exclude.txt"))
        {
            return false;
        }

        return true;
    }

    private List<WordCount> GetListOfWordCountForFile(string filePath)
    {
        return Regex.Matches(File.ReadAllText(filePath), @"\w+").Cast<Match>()
            .Select((m, pos) => new { Word = m.Value, Pos = pos })
            .GroupBy(s => s.Word, StringComparer.CurrentCultureIgnoreCase)
            .Select(g => new WordCount { Word = g.Key, Count = g.Select(z => z.Pos).ToList().Count })
            .ToList();
    }


    private void UpdateDictionaryWithList(IEnumerable<WordCount> words)
    {
        lock (wordOccurrence)
        {
            foreach (var item in words)
            {
                UpdateWordDictionary(item);
            }
        }
    }

    private void UpdateWordDictionary(WordCount item)
    {
        if (!wordOccurrence.ContainsKey(item.Word))
        {
            wordOccurrence.Add(item.Word, item.Count);
        }
        else
        {
            wordOccurrence[item.Word] += item.Count;
        }
    }




    private void ExcludeWordsFoundInExcludeFile()
    {
        var excludePath = $"{inputDirectory}/exclude.txt";

        if (!File.Exists(excludePath))
        {
            Console.WriteLine("exclude.txt does not exist!");
            return;
        }

        var excludedWords = GetAllWordsFromFile(excludePath);
        AddExcludedWordsToExcludedDictionary(excludedWords);
        foreach (var word in excludedWords)
        {
            ExcludeWordFromDictionary(word);
        }
    }

    private void AddExcludedWordsToExcludedDictionary(string[] excludedWords)
    {
        foreach (var word in excludedWords)
        {
            excludedWordOccurence.Add(word, 0);
        }
    }

    private string[] GetAllWordsFromFile(string path)
    {
        return Regex.Matches(File.ReadAllText(path), @"\w+").Cast<Match>()
            .Select(x => x.Value)
            .ToArray();
    }

    private void ExcludeWordFromDictionary(string word)
    {
        if (wordOccurrence.ContainsKey(word))
        {
            excludedWordOccurence[word] = wordOccurrence[word];
            wordOccurrence.Remove(word);
        }
    }

    private Task GenerateFileForExludedWords()
    {
        return Task.Run(() =>
        {
            var fileName = $"{inputDirectory}/excludedResult.txt";
            var fileContent = GenerateFileContentUsingDictionary(excludedWordOccurence);
            CreateFile(fileName, fileContent);
        });
    }

    private string[] GenerateFileContentUsingDictionary(Dictionary<string, int> dictionary)
    {
        var fileContent = new List<string>();
        foreach (var wordCount in dictionary)
        {
            var word = wordCount.Key;
            var count = wordCount.Value;
            fileContent.Add($"{word} {count}");
        }
        return fileContent.ToArray();
    }

    private Task GenerateFileForEachLetterInTheAplhabet()
    {
        var tasks = new List<Task>();

        for (char value = 'A'; value <= 'Z'; value++)
        {
            var letter = value;
            tasks.Add(Task.Run(() =>
            {
                var fileName = $"{inputDirectory}/{letter}.txt";
                var fileAppendDictionary = GetDictionaryWithFileAppend(letter);
                var fileContent = GenerateFileContentUsingDictionary(fileAppendDictionary);
                CreateFile(fileName, fileContent);
            }));
        }
        return Task.WhenAll(tasks);
    }

    private Dictionary<string, int> GetDictionaryWithFileAppend(char fileAppend)
    {
        var dictionary = new Dictionary<string, int>();

        foreach (var item in wordOccurrence)
        {
            if (item.Key.StartsWith(fileAppend))
            {
                dictionary.Add(item.Key, item.Value);
            }
        }
        return dictionary;
    }

    private void CreateFile(string fileName, string[] fileContent)
    {
        try
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                foreach (var wordCount in fileContent)
                {
                    sw.WriteLine(wordCount);
                }
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }
    }
}

