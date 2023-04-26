using Castle.Core.Internal;

namespace FileWordCounter.Tests;
public class WordOccurenceDictionaryTests
{
    private WordOccurenceDictionary wordOccurrenceDictionary;

    [SetUp]
    public void Setup()
    {
        wordOccurrenceDictionary = new WordOccurenceDictionary();
        wordOccurrenceDictionary.wordOccurrence = InitWordDictionary();

    }

    public Dictionary<string,int> InitWordDictionary()
    {
        var dictionary = new Dictionary<string, int>
        {
            { "a", 1 },
            { "b", 2 },
            { "c", 3 },
            { "d", 4 },
            { "e", 5 }
        };

        return dictionary;
    }

    [Test]
    public void ShouldExcludeWordsFromDictionary()
    {
        //arrange
        var excludeWord = new List<string> { "a" };

        //act
        wordOccurrenceDictionary.ExcludeWordsFromDictionary(excludeWord);

        //assert
        Assert.IsFalse(wordOccurrenceDictionary.wordOccurrence.ContainsKey("a"));

    }

    [Test]
    public void ShouldReturnExcludedWords()
    {
        //arrange
        var excludeWord = new List<string> { "a" };

        //act
        var excludedWords = wordOccurrenceDictionary.ExcludeWordsFromDictionary(excludeWord);

        //assert
        Assert.True(excludedWords.ContainsKey("a"));
    }

    [Test]
    public void ShouldReturnEmptyIfExcludedWordsAreEmpty()
    {
        //arrange
        var excludeWord = new List<string>();

        //act
        var excludedWords = wordOccurrenceDictionary.ExcludeWordsFromDictionary(excludeWord);

        //assert
        Assert.IsTrue(excludedWords.Count() == 0);
    }

    [Test]
    public void ShouldGetProperCountOfExcludedWordsFromDictionary()
    {
        //arrange
        var excludeWord = new List<string> { "a" };

        //act
        var excludedWords = wordOccurrenceDictionary.ExcludeWordsFromDictionary(excludeWord);

        //assert
        Assert.True(excludedWords["a"] == 1);
    }

    [Test]
    public void ShouldGetZeroCountIfNotInDictionary()
    {
        //arrange
        var excludeWord = new List<string> { "a","f" };

        //act
        var excludedWords = wordOccurrenceDictionary.ExcludeWordsFromDictionary(excludeWord);

        //assert
        Assert.True(excludedWords["f"] == 0);
    }

    [Test]
    public void ShouldReturnDictionaryOfWordsStatingWithCharA()
    {
        //arrange
        char append = "a"[0];

        //act
        var dictionOfChar = wordOccurrenceDictionary.GetDictionaryOfWordsStartingWith(append);

        //assert
        Assert.True(dictionOfChar.ContainsKey("a"));
    }
    
    [Test]
    public void ShouldReturnEmptyDictionaryOfWordsStatingWithCharZ()
    {
        //arrange
        char append = "Z"[0];

        //act
        var dictionOfChar = wordOccurrenceDictionary.GetDictionaryOfWordsStartingWith(append);

        //assert
        Assert.True(dictionOfChar.IsNullOrEmpty());
    }


    [Test]
    public void ShouldReturnEmptyDictionaryWhenCharIsEmpty()
    {
        //arrange
        char append = " "[0];

        //act
        var dictionOfChar = wordOccurrenceDictionary.GetDictionaryOfWordsStartingWith(append);

        //assert
        Assert.True(dictionOfChar.IsNullOrEmpty());
    }
}
