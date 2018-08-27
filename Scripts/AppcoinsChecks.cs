using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aptoide.AppcoinsUnity
{
    public static class AppcoinsChecks
    {
        public static APPCOINS_ERROR CheckSKUs(AppcoinsSku[] products)
        {
            if (products.Length == 0)
            {
                return APPCOINS_ERROR.NO_PRODUCTS;
            }

            else
            {
                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i] == null)
                    {
                        return APPCOINS_ERROR.PRODUCT_NULL;
                    }
                }
            }

            return APPCOINS_ERROR.NONE;
        }

        public static APPCOINS_ERROR CheckForRepeatedSkuId(
            AppcoinsSku[] products)
        {
            for (int i = 0; i < products.Length - 1; i++)
            {
                AppcoinsSku currentProduct = products[i];

                for (int j = i + 1; j < products.Length; j++)
                {
                    AppcoinsSku compareProduct = products[j];

                    if (currentProduct != null && currentProduct.SKUID.Length == 
                        compareProduct.SKUID.Length
                       )
                    {
                        if (currentProduct.SKUID.Equals(compareProduct.SKUID))
                        {
                            return APPCOINS_ERROR.PRODUCT_REPEATED;
                        }
                    }
                }
            }

            return APPCOINS_ERROR.NONE;
        }

        public static APPCOINS_ERROR CheckPoAActive(bool enablePOA)
        {
            if (enablePOA)
                return APPCOINS_ERROR.POA_ENABLED;

            return APPCOINS_ERROR.NONE;
        }

        // Makes sure that the prefab name is updated on the mainTemplat.gradle 
        // before the build process
        public static AppcoinsUnity ValidatePrefabName()
        {
            var foundObjects = Resources.FindObjectsOfTypeAll<AppcoinsUnity>();

            if (foundObjects.Length == 0)
            {
                UnityEngine.Debug.LogError("Found no object with component " +
                                           "AppcoinsUnity! Are you using the " +
                                           "prefab?");
                return null;
            }

            AppcoinsUnity appCoinsPrefabObject = foundObjects[0];

            string line;
            ArrayList fileLines = new ArrayList();

            System.IO.StreamReader fileReader = 
                new System.IO.StreamReader(Application.dataPath + 
                                           "/Plugins/Android/" +
                                           "mainTemplate.gradle");

            while ((line = fileReader.ReadLine()) != null)
            {
                if (line.Contains(AppcoinsUnity.APPCOINS_PREFAB))
                {
                    int i = 0;
                    string newLine = "";

                    while (line[i].Equals("\t") || line[i].Equals(" "))
                    {
                        i++;
                        newLine = string.Concat("\t", "");
                    }

                    newLine = string.Concat(newLine, line);

                    //Erase content after last comma
                    int lastComma = newLine.LastIndexOf(",");
                    newLine = newLine.Substring(0, lastComma + 1);
                    newLine = string.Concat(
                        newLine, 
                        " \"" + appCoinsPrefabObject.gameObject.name + "\""
                    );

                    fileLines.Add(newLine);
                }

                else
                {
                    fileLines.Add(line);
                }
            }

            fileReader.Close();

            System.IO.StreamWriter fileWriter = 
                new System.IO.StreamWriter(Application.dataPath + 
                                           "/Plugins/Android/" +
                                           "mainTemplate.gradle");

            foreach (string newLine in fileLines)
            {
                fileWriter.WriteLine(newLine);
            }

            fileWriter.Close();
            return appCoinsPrefabObject;
        }
    }
}
