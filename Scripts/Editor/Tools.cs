using System;
using System.IO;

public class Tools
{
    // Change a line in a file as much times as specified. 
    public static void ChangeLineInFile(string filePath, 
                                        string fileLine,
                                        string newLine,
                                        int times)
    {

    }

    public static string[] ReadFileToArray(string filePath)
    {
        StreamReader fileReader = new StreamReader(filePath);
        List<string> lines = new List<string>();

        string line;
        while((line = fileReader.ReadLine()) != null)
        {
            lines.Add(line);
        }

        fileReader.Close();
        return lines.ToArray();
    }

    public static void WriteToFile(string filePath, string[] lines)
    {
        Write(new StreamWriter(filePath, false), lines);
    }

    public static void AppendToFile(string filePath, string[] lines)
    {
        Write(new StreamWriter(filePath, true), lines);
    }

    private static void Write(StreamWriter fileWriter, 
                              string[] lines)
    {
        foreach (string line in lines)
        {
            fileWriter.WriteLine(line);
        }

        fileWriter.Close();
    }
}