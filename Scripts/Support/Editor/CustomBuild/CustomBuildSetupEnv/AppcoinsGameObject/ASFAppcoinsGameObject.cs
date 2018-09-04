using UnityEngine;
using Aptoide.AppcoinsUnity;

public class ASFAppcoinsGameObject : AppcoinsGameObject
{
    private AppcoinsUnity asfGameObject;
    private const string appcoinsNameNewLine = "resValue \"string\", " +
        "\"APPCOINS_PREFAB\", \"{0}\"";

    public ASFAppcoinsGameObject()
    {
        FindAppcoinsGameObject();
    }

    private void FindAppcoinsGameObject()
    {
        var foundObjects = Resources.FindObjectsOfTypeAll<AppcoinsUnity>();

        if (foundObjects.Length == 0)
        {
            throw new ASFAppcoinsGameObjectNotFound();
        }

        asfGameObject = foundObjects[0];
    }

    internal override void CheckAppcoinsGameobject()
    {
        string newLine = 
            appcoinsNameNewLine.Replace(toReplace, 
                                        asfGameObject.gameObject.name);

        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateVarName,
                               mainTemplateContainers, newLine, numTimes);
    }
}
