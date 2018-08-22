using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;
using System.IO;

public class Tools
{
    // Change a line in a file as much times as specified. 
    // file has the same format as a .gradle file
    public static void ChangeLineInFile(string filePath, 
                                        string fileLine,
                                        string[] containers,
                                        string newLine,
                                        int times)
    {
        string[] fileLines = ReadFileToArray(filePath);
        int linesChanged = 0;
        bool insideContainer = false;

        for (int i = 0; i < fileLines.Length; i++)
        {
            foreach (string container in containers)
            {
                if (fileLines[i].Contains(container))
                {
                    insideContainer = true;
                }
            }

            if (linesChanged < times && insideContainer && 
                fileLines[i].Contains(fileLine))
            {
                // Retrive the number of tabs before the line just 
                // to maintain the same format (we assume that
                // only exists one 'command / definition' per line)
                int tabsNum = 0;
                while (Char.IsWhiteSpace(fileLines[i][tabsNum++]));

                fileLines[i] = String.Concat(
                                    fileLines[i].Substring(0, tabsNum),
                                    newLine);
                
                linesChanged++;
                insideContainer = false;
            }
        }

        WriteToFile(filePath, fileLines);
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

    public static string GetProjectPath()
    {
        string projPath = Application.dataPath;

        int index = projPath.LastIndexOf('/');
        projPath = projPath.Substring(0, index);

        return projPath;
    }

    // If folder already exists in the chosen directory delete it.
    protected void DeleteIfFolderAlreadyExists(string path)
    {
        string[] folders = Directory.GetDirectories(path);

        for (int i = 0; i < folders.Length; i++)
        {
            if ((new DirectoryInfo(folders[i]).Name) == 
                PlayerSettings.productName
               )
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folders[i]);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
    }
}