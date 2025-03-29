using System;
using System.Collections.Generic;
using System.IO;

class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Text File Editor and Indexer");
    Console.WriteLine("----------------------------");

    FileTextSearcher searcher = new FileTextSearcher();
    TextFileEditor editor = null;

    while (true)
    {
      Console.WriteLine("\nMenu:");
      Console.WriteLine("1. Index files in directory");
      Console.WriteLine("2. Search files by keywords");
      Console.WriteLine("3. Edit text file");
      Console.WriteLine("4. Exit");
      Console.Write("Select option: ");

      string choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          HandleIndexing(searcher);
          break;

        case "2":
          HandleSearch(searcher);
          break;

        case "3":
          editor = HandleFileEditing();
          break;

        case "4":
          return;

        default:
          Console.WriteLine("Invalid option. Try again.");
          break;
      }
    }
  }

  private static void HandleIndexing(FileTextSearcher searcher)
  {
    Console.Write("Enter directory path: ");
    string directoryPath = Console.ReadLine();
    
    Console.Write("Enter keywords (comma separated): ");
    string keywordsInput = Console.ReadLine();
    
    List<string> keywords = new List<string>(keywordsInput.Split(','));
    searcher.BuildIndex(directoryPath, keywords);
    
    Console.WriteLine("Indexing completed.");
  }

  private static void HandleSearch(FileTextSearcher searcher)
  {
    Console.Write("Enter keyword to search: ");
    string searchKeyword = Console.ReadLine();
    
    List<string> foundFiles = searcher.SearchFilesContainingKeyword(searchKeyword);
    
    Console.WriteLine($"Found {foundFiles.Count} files:");
    foreach (string file in foundFiles)
    {
      Console.WriteLine(file);
    }
  }

  private static TextFileEditor HandleFileEditing()
  {
    Console.Write("Enter file path to edit: ");
    string filePath = Console.ReadLine();
    
    if (!File.Exists(filePath))
    {
      Console.WriteLine("File does not exist. Create new file? (y/n)");
      if (Console.ReadLine().ToLower() == "y")
      {
        File.WriteAllText(filePath, "");
      }
      else
      {
        return null;
      }
    }
    
    FileText textFile = FileText.LoadFromFile(filePath);
    TextFileEditor editor = new TextFileEditor(textFile);
    
    RunEditorMenu(editor);
    return editor;
  }

  private static void RunEditorMenu(TextFileEditor editor)
  {
    while (true)
    {
      Console.WriteLine("\nFile Editor Menu:");
      Console.WriteLine("1. View content");
      Console.WriteLine("2. Edit content");
      Console.WriteLine("3. Insert text");
      Console.WriteLine("4. Delete text");
      Console.WriteLine("5. Undo");
      Console.WriteLine("6. Redo");
      Console.WriteLine("7. Save");
      Console.WriteLine("8. Back to main menu");
      Console.Write("Select option: ");

      string choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          Console.WriteLine("\nFile content:");
          editor.PrintContent();
          break;

        case "2":
          Console.WriteLine("Enter new content:");
          string newContent = Console.ReadLine();
          editor.EditContent(newContent);
          Console.WriteLine("Content updated.");
          break;

        case "3":
          HandleInsertText(editor);
          break;

        case "4":
          HandleDeleteText(editor);
          break;

        case "5":
          editor.Undo();
          Console.WriteLine("Undo completed.");
          break;

        case "6":
          editor.Redo();
          Console.WriteLine("Redo completed.");
          break;

        case "7":
          editor.Save();
          Console.WriteLine("File saved.");
          break;

        case "8":
          return;

        default:
          Console.WriteLine("Invalid option. Try again.");
          break;
      }
    }
  }

  private static void HandleInsertText(TextFileEditor editor)
  {
    Console.Write("Enter text to insert: ");
    string insertText = Console.ReadLine();
    
    Console.Write("Enter position: ");
    if (int.TryParse(Console.ReadLine(), out int position))
    {
      editor.InsertText(insertText, position);
      Console.WriteLine("Text inserted.");
    }
    else
    {
      Console.WriteLine("Invalid position.");
    }
  }

  private static void HandleDeleteText(TextFileEditor editor)
  {
    Console.Write("Enter start index: ");
    if (int.TryParse(Console.ReadLine(), out int startIndex))
    {
      Console.Write("Enter length: ");
      if (int.TryParse(Console.ReadLine(), out int length))
      {
        editor.DeleteText(startIndex, length);
        Console.WriteLine("Text deleted.");
      }
      else
      {
        Console.WriteLine("Invalid length.");
      }
    }
    else
    {
      Console.WriteLine("Invalid start index.");
    }
  }
}