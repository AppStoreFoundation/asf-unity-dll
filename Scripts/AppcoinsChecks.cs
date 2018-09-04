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
    }
}
