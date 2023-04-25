using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileWordCounter
{
    public class WordOccurenceDictionary
    {
        public Dictionary<string,int> wordOccurrence { get; set; }

        public WordOccurenceDictionary()
        {
            wordOccurrence = new();
        }

        public Dictionary<string,int> ExcludeWordsFromDictionary(List<WordCount> excludedWords) 
        {
            Dictionary<string,int> excludedWordsDictionary = new();
            excludedWords.ForEach(item =>
            {
                if (wordOccurrence.ContainsKey(item.Word))
                {
                    excludedWordsDictionary[item.Word] = wordOccurrence[item.Word];
                    wordOccurrence.Remove(item.Word);
                }
                else
                {
                    excludedWordsDictionary[item.Word] = 0;
                }
            });
            return excludedWordsDictionary;   
        }

        public Dictionary<string, int> GetDictionaryOfWordsStartingWith(char fileAppend)
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
    }


}
