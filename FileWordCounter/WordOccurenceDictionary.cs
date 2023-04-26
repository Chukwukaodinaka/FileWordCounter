namespace FileWordCounter;

public class WordOccurenceDictionary
{
    public Dictionary<string,int> wordOccurrence { get; set; }

    public WordOccurenceDictionary()
    {
        wordOccurrence = new();
    }

    public Dictionary<string,int> ExcludeWordsFromDictionary(List<string> excludedWords) 
    {
      

        Dictionary<string,int> excludedWordsDictionary = new();
        excludedWords.ForEach(item =>
        {
            if (wordOccurrence.ContainsKey(item))
            {
                excludedWordsDictionary[item] = wordOccurrence[item];
                wordOccurrence.Remove(item);
            }
            else
            {
                excludedWordsDictionary[item] = 0;
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
