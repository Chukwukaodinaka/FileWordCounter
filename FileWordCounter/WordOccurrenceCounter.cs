using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileWordCounter
{
    public class WordOccurrenceCounter
    {
        public static void CountOccurrenceForEachWordFromList(string[] fileData, Dictionary<string, int> wordOccurrence)
        {
            foreach (var fileContent in fileData)
            {
                var wordCount = CountOccurrenceForEachWord(fileContent);
                UpdateListWithOccurence(wordCount,wordOccurrence);
            }
        }

        public static void UpdateListWithOccurence(List<WordCount> wordCounts,Dictionary<string,int> wordOccurrence)
        {
            lock (wordOccurrence)
            {
                foreach (var item in wordCounts)
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
            }   
        }

        public static List<WordCount> CountOccurrenceForEachWord(string content)
        {
            return Regex.Matches(content, @"\w+").Cast<Match>()
                 .Select((m, pos) => new { Word = FirstLetterToUppercase(m.Value), Pos = pos })
                 .GroupBy(s => s.Word, StringComparer.CurrentCultureIgnoreCase)
                 .Select(g => new WordCount { Word = g.Key, Count = g.Select(z => z.Pos).ToList().Count })
                 .ToList();
        }

        private static string FirstLetterToUppercase(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }
    }
}
