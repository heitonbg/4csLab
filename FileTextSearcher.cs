using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextFileEditor
{
  public class FileTextSearcher
  {
    public Dictionary<string, List<string>> Index { get; private set; } = new Dictionary<string, List<string>>();

    public void BuildIndex(string directoryPath, List<string> keywords)
    {
      Index.Clear();
      
      foreach (string keyword in keywords)
      {
        Index[keyword] = new List<string>();
      }

      foreach (string filePath in Directory.GetFiles(directoryPath, "*.txt"))
      {
        string content = File.ReadAllText(filePath).ToLower();
        
        foreach (string keyword in keywords)
        {
          if (content.Contains(keyword.ToLower()))
          {
            Index[keyword].Add(filePath);
          }
        }
      }
    }

    public List<string> SearchFilesContainingKeyword(string keyword)
    {
      if (Index.TryGetValue(keyword, out List<string> fileList))
      {
        return fileList;
      }
      return new List<string>();
    }

    public List<string> SearchFilesContainingAllKeywords(List<string> keywords)
    {
      List<string> result = new List<string>();
      bool isFirstKeyword = true;

      foreach (string keyword in keywords)
      {
        if (!Index.ContainsKey(keyword))
        {
          return new List<string>();
        }

        if (isFirstKeyword)
        {
          result.AddRange(Index[keyword]);
          isFirstKeyword = false;
        }
        else
        {
          result = result.Intersect(Index[keyword]).ToList();
        }
      }

      return result.Distinct().ToList();
    }
  }
}