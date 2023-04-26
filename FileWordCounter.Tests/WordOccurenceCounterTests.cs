namespace FileWordCounter.Tests;

public class WordOccurenceCounterTests
{

    [Test]
    public void ShouldUpdateDictionaryForEachFileData()
    {
        //arrange 
        var fileData = new string[] { "a", "a", "a", "a" };

        //act
        var dictionary = WordOccurrenceCounter.CountOccurrenceForEachWordFromList(fileData);

        //assert
        Assert.IsTrue(dictionary.wordOccurrence.ContainsKey("A"));
        Assert.IsTrue(dictionary.wordOccurrence["A"] == 4);
    }

    [Test]
    public void ShouldBeSameOccurenceForWordsWithDifferentCase()
    {
        //arrange 
        var fileData = new string[] { "dog", "Dog", "dOg", "doG" };

        //act
        var dictionary = WordOccurrenceCounter.CountOccurrenceForEachWordFromList(fileData);

        //assert
        Assert.IsTrue(dictionary.wordOccurrence.ContainsKey("Dog"));
        Assert.IsTrue(dictionary.wordOccurrence["Dog"] == 4);
    }

    [Test]
    public void ShouldAddWordCountForIfNotInDictionary()
    {
        //arrange 
        var fileData = new string[] { "dog" };

        //act
        var dictionary = WordOccurrenceCounter.CountOccurrenceForEachWordFromList(fileData);

        //assert
        Assert.IsTrue(dictionary.wordOccurrence.ContainsKey("Dog"));
        Assert.IsTrue(dictionary.wordOccurrence["Dog"] == 1);
    }

    [Test]
    public void ShouldGetEachWordInContent()
    {
        //arrange
        var fileContent = "cat dog";

        //act
        var words = WordOccurrenceCounter.GetEachWordInContent(fileContent);

        //assert
        Assert.IsTrue(words.Contains("Cat"));
        Assert.IsTrue(words.Contains("Dog"));
    }
}
