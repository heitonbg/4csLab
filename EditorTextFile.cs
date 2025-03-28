using System;
using System.Collections.Generic;

public class TextFileMemento
{
  public string Content { get; }
  public DateTime SnapshotDate { get; }

  public TextFileMemento(string content)
  {
    Content = content;
    SnapshotDate = DateTime.Now;
  }
}

public class TextFileEditor
{
  private FileText _textFile;
  private Stack<TextFileMemento> _undoStack = new Stack<TextFileMemento>();
  private Stack<TextFileMemento> _redoStack = new Stack<TextFileMemento>();

  public TextFileEditor(FileText textFile)
  {
    _textFile = textFile;
    SaveState();
  }

  public void EditContent(string newContent)
  {
    SaveState();
    _textFile.Content = newContent;
    _redoStack.Clear();
  }

  public void InsertText(string text, int position)
  {
    if (position < 0 || position > _textFile.Content.Length)
    {
      throw new ArgumentOutOfRangeException(nameof(position));
    }

    SaveState();
    _textFile.Content = _textFile.Content.Insert(position, text);
    _redoStack.Clear();
  }

  public void DeleteText(int startIndex, int length)
  {
    if (startIndex < 0 || startIndex >= _textFile.Content.Length)
    {
      throw new ArgumentOutOfRangeException(nameof(startIndex));
    }

    SaveState();
    _textFile.Content = _textFile.Content.Remove(startIndex, length);
    _redoStack.Clear();
  }

  public void Undo()
  {
    if (_undoStack.Count > 1)
    {
      TextFileMemento currentState = _undoStack.Pop();
      _redoStack.Push(currentState);
      _textFile.Content = _undoStack.Peek().Content;
    }
  }

  public void Redo()
  {
    if (_redoStack.Count > 0)
    {
      TextFileMemento redoState = _redoStack.Pop();
      _undoStack.Push(redoState);
      _textFile.Content = redoState.Content;
    }
  }

  public void Save()
  {
    _textFile.SaveToFile();
  }

  private void SaveState()
  {
    _undoStack.Push(new TextFileMemento(_textFile.Content));
  }

  public void PrintContent()
  {
    Console.WriteLine(_textFile.Content);
  }
}