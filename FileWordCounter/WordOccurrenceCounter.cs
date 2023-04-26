using System.Text.RegularExpressions;

namespace FileWordCounter;

public class WordOccurrenceCounter
{
    public static WordOccurenceDictionary CountOccurrenceForEachWordFromList(string[] fileData)
    {
        WordOccurenceDictionary dictionary = new();

        foreach (var fileContent in fileData)
        {
            var wordCount = CountOccurrenceForEachWord(fileContent);
            UpdateListWithOccurence(wordCount,dictionary.wordOccurrence);
        }

        return dictionary;
    }

    public static void UpdateListWithOccurence(Dictionary<string,int> wordCounts,Dictionary<string,int> wordOccurrence)
    {
        lock (wordOccurrence)
        {
            foreach (var item in wordCounts)
            {
                if (!wordOccurrence.ContainsKey(item.Key))
                {
                    wordOccurrence.Add(item.Key, item.Value);
                }
                else
                {
                    wordOccurrence[item.Key] += item.Value;
                }
            }
        }   
    }

    public static Dictionary<string,int> CountOccurrenceForEachWord(string content)
    {
        Dictionary<string, int> wordOccurence = new();

         Regex.Matches(content, @"\w+").Cast<Match>()
             .Select((m, pos) => new { Word = ToProperCases(m.Value), Pos = pos })
             .GroupBy(s => s.Word, StringComparer.CurrentCultureIgnoreCase)
             .ToList()
             .ForEach(x => wordOccurence.Add(x.Key, x.Select(z => z.Pos).ToList().Count));

        return wordOccurence;
    }

    private static string ToProperCases(string value)
    {
        return FirstLetterToUppercase(value) + value.Substring(1).ToLower();
    }

    private static char FirstLetterToUppercase(string value)
    {
        return char.ToUpper(value[0]) ;
    }

    public static List<string> GetEachWordInContent(string content)
    {
       return Regex.Matches(content, @"\w+").Cast<Match>()
             .Select(x => ToProperCases(x.Value))
             .ToList(); 
    }        
}
