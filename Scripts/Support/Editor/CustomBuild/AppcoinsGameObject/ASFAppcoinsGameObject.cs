using UnityEngine;

using System;

using Aptoide.AppcoinsUnity;

public class ASFAppcoinsGameObject : AppcoinsGameObject
{
    private ASFAppcoinsUnity asfGameObject;
    private const string appcoinsPOA = "APPCOINS_ENABLE_POA";
    private const string appcoinsDebug = "APPCOINS_ENABLE_DEBUG";

    private const string appcoinsPOANewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_POA\", \"{0}\"";
    private const string appcoinsDebugNewLine = "resValue \"bool\", " +
        "\"APPCOINS_ENABLE_DEBUG\", \"{0}\"";
    private const string appcoinsNameNewLine = "resValue \"string\", " +
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

        //  Change Appcoins prefab name in mainTempla.gradle
        string newNameLine =
            appcoinsNameNewLine.Replace(toReplace,
                                        asfGameObject.gameObject.name);
        string newPOALine =
            appcoinsPOANewLine.Replace(toReplace,
                                       asfGameObject.enablePOA.ToString()
                                            .ToLower()
                                      );

        string newDebugLine =
            appcoinsDebugNewLine.Replace(toReplace,
                                         asfGameObject.enableDebug.ToString()
                                            .ToLower()
                                        );

        Tools.ChangeLineInFile(mainTemplatePath, mainTemplateVarName,
                               mainTemplateContainers, newNameLine, numTimes);
        Tools.ChangeLineInFile(mainTemplatePath, appcoinsPOA,
                               mainTemplateContainers, newPOALine, numTimes);
        Tools.ChangeLineInFile(mainTemplatePath, appcoinsDebug,
                               mainTemplateContainers, newDebugLine, numTimes);

        //// Check Appcoins prefab's products
        //try
        //{
        //    AppcoinsChecks.CheckForRepeatedSkuId(asfGameObject.GetProductList());
        //    AppcoinsChecks.CheckSKUs(asfGameObject.GetProductList());
        //}
        //catch (NoSKUProductsException e)
        //{
        //    throw new Exception(e.message);
        //}
        //catch (NullSKUProductException e)
        //{
        //    throw new Exception(e.message);
        //}
        //catch (RepeatedSKUProductException e)
        //{
        //    throw new Exception(e.message);
        //}
    }
}
