using System;

using Aptoide.AppcoinsUnity;

public class ASFAppcoinsGameObject : AppcoinsGameObject
{
    private ASFAppcoinsUnity asfGameObject;
    private const string appcoinsPrefab = "APPCOINS_PREFAB";
    private const string appcoinsPOA = "APPCOINS_ENABLE_POA";
    private const string appcoinsDebug = "APPCOINS_ENABLE_DEBUG";

    private const string poaNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_POA\", \"{0}\"";
    private const string debugNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_DEBUG\", \"{0}\"";
    private const string nameNewLine = "resValue \"string\", " +
        "\"APPCOINS_PREFAB\", \"{0}\"";

    private void FindAppcoinsGameObject()
    {
        ASFAppcoinsUnity[] foundObjects = (ASFAppcoinsUnity[])
            UnityEngine.Object.FindObjectsOfType(typeof(ASFAppcoinsUnity));

        if (foundObjects.Length == 0)
        {
            throw new Exception(new ASFAppcoinsGameObjectNotFound().message);
        }

        asfGameObject = foundObjects[0];
    }

    public override void CheckAppcoinsGameobject()
    {
        FindAppcoinsGameObject();

        asfGameObject.CheckWalletAddress();

        string[] newLines = {
            nameNewLine.Replace(toReplace, asfGameObject.gameObject.name),
            poaNewLine.Replace(toReplace,
                               asfGameObject.enablePOA.ToString().ToLower()),
            debugNewLine.Replace(toReplace,
                                 asfGameObject.enableDebug.ToString().ToLower())
        };

        //  Change Appcoins prefab name in mainTempla.gradle
        changeLineInMainTemplate(mainTemplatePath, mainTemplateContainers,
                                 newLines);
    }

    private void changeLineInMainTemplate(string filePath, string[] containers,
                                          string[] newLines)
    {
        Tools.RemoveLineInFileWithSpecString(filePath, newLines);
        Tools.AddLinesInFile(filePath, containers, newLines);
    }
}
