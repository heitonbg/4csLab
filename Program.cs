using System;
using System.Collections.Generic;
using System.IO;

namespace TextFileEditor
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Текстовый редактор с индексацией файлов");
      Console.WriteLine("---------------------------------------");

      FileTextSearcher searcher = new FileTextSearcher();
      TextFileEditor editor = null;

      while (true)
      {
        Console.WriteLine("\nГлавное меню:");
        Console.WriteLine("1. Индексировать файлы в директории");
        Console.WriteLine("2. Поиск файлов по ключевым словам");
        Console.WriteLine("3. Редактировать текстовый файл");
        Console.WriteLine("4. Выход");
        Console.Write("Выберите действие: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
          case "1":
            ОбработатьИндексацию(searcher);
            break;

          case "2":
            ОбработатьПоиск(searcher);
            break;

          case "3":
            editor = ОбработатьРедактирование();
            break;

          case "4":
            return;

          default:
            Console.WriteLine("Неверный выбор. Попробуйте снова.");
            break;
        }
      }
    }

    private static void ОбработатьИндексацию(FileTextSearcher searcher)
    {
      Console.Write("Введите путь к директории: ");
      string directoryPath = Console.ReadLine();
      
      Console.Write("Введите ключевые слова (через запятую): ");
      string keywordsInput = Console.ReadLine();
      
      List<string> keywords = new List<string>(keywordsInput.Split(','));
      searcher.BuildIndex(directoryPath, keywords);
      
      Console.WriteLine("Индексация завершена.");
    }

    private static void ОбработатьПоиск(FileTextSearcher searcher)
    {
      Console.Write("Введите ключевое слово для поиска: ");
      string searchKeyword = Console.ReadLine();
      
      List<string> foundFiles = searcher.SearchFilesContainingKeyword(searchKeyword);
      
      Console.WriteLine($"Найдено файлов: {foundFiles.Count}");
      foreach (string file in foundFiles)
      {
        Console.WriteLine(file);
      }
    }

    private static TextFileEditor ОбработатьРедактирование()
    {
      Console.Write("Введите путь к файлу для редактирования: ");
      string filePath = Console.ReadLine();
      
      if (!File.Exists(filePath))
      {
        Console.WriteLine("Файл не существует. Создать новый? (д/н)");
        if (Console.ReadLine().ToLower() == "д")
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
      
      ЗапуститьМенюРедактора(editor);
      return editor;
    }

    private static void ЗапуститьМенюРедактора(TextFileEditor editor)
    {
      while (true)
      {
        Console.WriteLine("\nМеню редактора:");
        Console.WriteLine("1. Просмотреть содержимое");
        Console.WriteLine("2. Редактировать содержимое");
        Console.WriteLine("3. Вставить текст");
        Console.WriteLine("4. Удалить текст");
        Console.WriteLine("5. Отменить (Undo)");
        Console.WriteLine("6. Повторить (Redo)");
        Console.WriteLine("7. Сохранить");
        Console.WriteLine("8. Вернуться в главное меню");
        Console.Write("Выберите действие: ");

        string choice = Console.ReadLine();

        switch (choice)
        {
          case "1":
            Console.WriteLine("\nСодержимое файла:");
            editor.PrintContent();
            break;

          case "2":
            Console.WriteLine("Введите новое содержимое:");
            string newContent = Console.ReadLine();
            editor.EditContent(newContent);
            Console.WriteLine("Содержимое обновлено.");
            break;

          case "3":
            ОбработатьВставкуТекста(editor);
            break;

          case "4":
            ОбработатьУдалениеТекста(editor);
            break;

          case "5":
            editor.Undo();
            Console.WriteLine("Отмена выполнена.");
            break;

          case "6":
            editor.Redo();
            Console.WriteLine("Повтор выполнен.");
            break;

          case "7":
            editor.Save();
            Console.WriteLine("Файл сохранён.");
            break;

          case "8":
            return;

          default:
            Console.WriteLine("Неверный выбор. Попробуйте снова.");
            break;
        }
      }
    }

    private static void ОбработатьВставкуТекста(TextFileEditor editor)
    {
      Console.Write("Введите текст для вставки: ");
      string insertText = Console.ReadLine();
      
      Console.Write("Введите позицию: ");
      if (int.TryParse(Console.ReadLine(), out int position))
      {
        editor.InsertText(insertText, position);
        Console.WriteLine("Текст вставлен.");
      }
      else
      {
        Console.WriteLine("Неверная позиция.");
      }
    }

    private static void ОбработатьУдалениеТекста(TextFileEditor editor)
    {
      Console.Write("Введите начальный индекс: ");
      if (int.TryParse(Console.ReadLine(), out int startIndex))
      {
        Console.Write("Введите длину: ");
        if (int.TryParse(Console.ReadLine(), out int length))
        {
          editor.DeleteText(startIndex, length);
          Console.WriteLine("Текст удалён.");
        }
        else
        {
          Console.WriteLine("Неверная длина.");
        }
      }
      else
      {
        Console.WriteLine("Неверный начальный индекс.");
      }
    }
  }
}