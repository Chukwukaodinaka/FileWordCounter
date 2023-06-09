﻿namespace FileWordCounter;

public class FileHandler
{
    public static async Task<string[]> GetDataFromFilesInDirectory(string directory)
    {
        List<Task> tasks = new();
        var fileDatas = new List<string>();
        var filePaths = GetFilesOfDirectory(directory);
        foreach (var filePath in filePaths)
        {
            
            tasks.Add(Task.Run(() => {
                if(FileExistsAndIsNotExclude(filePath))
                {
                    var fileData = File.ReadAllText(filePath);
                    lock (fileDatas)
                    {
                        fileDatas.Add(fileData);
                    }
                }           
            }));
            
        }
        await Task.WhenAll(tasks);
        return fileDatas.ToArray();
    }

    public static string GetTextsFromFile(string path)
    {
        if (!File.Exists(path))
        {
            return default;
        }
        return File.ReadAllText(path);
    }

    public static void CreateFile(string fileName, string[] fileContent)
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

    public static string[] GenerateFileContentUsingDictionary(Dictionary<string, int> dictionary)
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

    private static string[]? GetFilesOfDirectory(string? directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            return default;
        }
        return Directory.GetFiles(directoryPath);
    }

    private static bool FileExistsAndIsNotExclude(string filePath)
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
}
