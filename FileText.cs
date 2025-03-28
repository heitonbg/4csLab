using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace TextFileEditor
{
  [Serializable]
  public class FileText
  {
    public string FilePath { get; set; }
    public string Content { get; set; }
    public DateTime LastModified { get; set; }

    public FileText() { }

    public FileText(string filePath, string content)
    {
      FilePath = filePath;
      Content = content;
      LastModified = DateTime.Now;
    }

    public void BinarySerialize(string filePath)
    {
      IFormatter formatter = new BinaryFormatter();
      using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
      {
        formatter.Serialize(stream, this);
      }
    }

    public static FileText BinaryDeserialize(string filePath)
    {
      IFormatter formatter = new BinaryFormatter();
      using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
      {
        return (FileText)formatter.Deserialize(stream);
      }
    }

    public void XmlSerialize(string filePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(FileText));
      using (TextWriter writer = new StreamWriter(filePath))
      {
        serializer.Serialize(writer, this);
      }
    }

    public static FileText XmlDeserialize(string filePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(FileText));
      using (TextReader reader = new StreamReader(filePath))
      {
        return (FileText)serializer.Deserialize(reader);
      }
    }

    public void SaveToFile()
    {
      File.WriteAllText(FilePath, Content);
      LastModified = DateTime.Now;
    }

    public static FileText LoadFromFile(string filePath)
    {
      return new FileText(filePath, File.ReadAllText(filePath));
    }
  }
}