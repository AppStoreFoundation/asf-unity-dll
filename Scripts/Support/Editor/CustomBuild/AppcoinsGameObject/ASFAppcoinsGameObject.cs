using UnityEngine;

using System;

using Aptoide.AppcoinsUnity;

public class ASFAppcoinsGameObject : AppcoinsGameObject
{
    private AppcoinsUnity asfGameObject;
    private const string appcoinsPOA = "APPCOINS_ENABLE_POA";
    private const string appcoinsDebug = "APPCOINS_ENABLE_DEBUG";

    private const string appcoinsPOANewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_POA\", \"{0}\"";
    private const string appcoinsDebugNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_DEBUG\", \"{0}\"";
    private const string appcoinsNameNewLine = "resValue \"string\", " +
        "\"APPCOINS_PREFAB\", \"{0}\"";

    public ASFAppcoinsGameObject()
    {
        FindAppcoinsGameObject();
    }

    private void FindAppcoinsGameObject()
    {
        AppcoinsUnity[] foundObjects = (AppcoinsUnity[])
            UnityEngine.Object.FindObjectsOfType(typeof(AppcoinsUnity));

        if (foundObjects.Length == 0)
        {
            throw new ASFAppcoinsGameObjectNotFound();
        }

        asfGameObject = foundObjects[0];
    }

    public override void CheckAppcoinsGameobject()
    {
        //  Change Appcoins prefab name in mainTempla.gradle
        string newNameLine = 
            appcoinsNameNewLine.Replace(toReplace, 
                                        asfGameObject.gameObject.name);
        string newPOALine = 
            appcoinsPOANewLine.Replace(toReplace, 
                                       asfGameObject.enablePOA.ToString()
                                      );

        string newDebugLine = 
            appcoinsDebugNewLine.Replace(toReplace, 
                                         asfGameObject.enableDebug.ToString()
                                        );

        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateVarName,
                               mainTemplateContainers, newNameLine, numTimes);
        Tools.ChangeLineInFile(mainTemplatePath, appcoinsPOA,
                               mainTemplateContainers, newPOALine, numTimes);
        Tools.ChangeLineInFile(mainTemplatePath, appcoinsDebug,
                               mainTemplateContainers, newDebugLine, numTimes);

        // Check Appcoins prefab's products
        try
        {
            AppcoinsChecks.CheckForRepeatedSkuId(asfGameObject.products);
            AppcoinsChecks.CheckSKUs(asfGameObject.products);
        }
        catch (NoProductsException e)
        {
            Debug.Log(e.message);
            throw new Exception(e.message);
        }
        catch (NullProductException e)
        {
            Debug.Log(e.message);
            throw new Exception(e.message);
        }
        catch (RepeatedProductException e)
        {
            throw new Exception(e.message);
        }
    }
}
