using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aptoide.AppcoinsUnity
{
    public static class AppcoinsChecks
    {
        public static void DefaultFullCheck(AppcoinsSku[] products)
        {
            MessageHandler messageHandler = new DisplayDialogMessageHandler();
            string ok = "ok";
            string title = "AppCoins Checker";

            try
            {
                CheckSKUs(products);
                CheckForRepeatedSkuId(products);
            }
            catch (NoProductsException e)
            {
                messageHandler.HandleMessage(title, e.message, ok);
                UnityEditor.EditorApplication.isPlaying = false;
            }
            catch (NullProductException e)
            {
                messageHandler.HandleMessage(title, e.message, ok);
                UnityEditor.EditorApplication.isPlaying = false;
            }
            catch (RepeatedProductException e)
            {
                messageHandler.HandleMessage(title, e.message, ok);
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

        public static void CheckSKUs(AppcoinsSku[] products)
        {
            if (products.Length == 0)
            {
                throw new NoProductsException();
            }

            else
            {
                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i] == null)
                    {
                        throw new NullProductException();
                    }
                }
            }
        }

        public static void CheckForRepeatedSkuId(
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
                            throw new RepeatedProductException();
                        }
                    }
                }
            }
        }

        public static bool CheckPoAActive(bool enablePOA)
        {
            if (enablePOA)
                return true;

            return false;
        }
    }
}
