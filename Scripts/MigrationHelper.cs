using UnityEngine;
using UnityEditor;

using System;
using System.IO;

[InitializeOnLoad]
public class MigrationHelper
{

    static MigrationHelper()
    {
        Debug.Log("MigrationHelper is running");

        string scriptsPath = Application.dataPath + "/AppcoinsUnity/Scripts";
        string editorScriptsPath = scriptsPath + "/Editor";

        Debug.Log(scriptsPath);
        Debug.Log(editorScriptsPath);

        string[] olderScriptFiles = { "AppcoinsPurchaser", "AppcoinsSku",
            "AppcoinsUnity", "BashUtils",
            "AppCoinsUnityPluginTests2018",
            "AppCoinsUnityPluginTests2017",
            "AppCoinsUnityPluginTests5_6",
            "AppCoinsUnityPluginEditorMode2018",
            "AppCoinsUnityPluginEditorMode5_6",
            "AppCoinsUnityPluginEditorMode2017" };

        string[] olderEditorScriptFiles = { "AppCoinsProductEditor",
            "AppcoinsStartup", "CustomBuild", "CustomTree",
            "ProductMaker", "appcoins-unity-support-5_6",
            "appcoins-unity-support-2017", "AppcoinsUnitySupport2018",
            "AppCoinsUnitySupport2017", "AppCoinsUnitySupport2018",
            "AppCoinsUnitySupport5_6" };

        DeleteFiles(scriptsPath, olderScriptFiles);
        DeleteFiles(editorScriptsPath, olderEditorScriptFiles);
    }

    private static void DeleteFiles(string dirPath, string[] filesToDelete)
    {
        foreach (string filePath in Directory.GetFiles(dirPath))
        {
            string fName = Path.GetFileName(filePath);
            Debug.Log(fName);

            if (Array.Exists(filesToDelete, dF => dF.Equals(fName)))
            {
                File.Delete(filePath);
            }
        }
    }
}
