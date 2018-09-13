using UnityEngine;
using System;
using System.Collections.Generic;

namespace Aptoide.AppcoinsUnity
{
    public static class AppcoinsChecks
    {
        public static void DefaultFullCheck(List<AppcoinsSKU> products)
        {
            try
            {
                CheckSKUs(products);
                CheckForRepeatedSkuId(products);
            }
            catch (NoSKUProductsException e)
            {
                throw new Exception(e.message);
            }
            catch (NullSKUProductException e)
            {
                throw new Exception(e.message);
            }
            catch (RepeatedSKUProductException e)
            {
                throw new Exception(e.message);
            }
        }

        public static void CheckSKUs(List<AppcoinsSKU> products)
        {
            if (products.Count == 0)
            {
                throw new NoSKUProductsException();
            }

            else
            {
                foreach (AppcoinsSKU product in products)
                {
                    if (product == null)
                    {
                        throw new NullSKUProductException();
                    }
                }
            }
        }

        public static void CheckForRepeatedSkuId(List<AppcoinsSKU> products)
        {
            for (int i = 0; i < products.Capacity - 1; i++)
            {
                AppcoinsSKU currentProduct = products[i];

                for (int j = i + 1; j < products.Count; j++)
                {
                    AppcoinsSKU compareProduct = products[j];

                    if (currentProduct != null && currentProduct.SKUID.Length == 
                        compareProduct.SKUID.Length
                       )
                    {
                        if (currentProduct.SKUID.Equals(compareProduct.SKUID))
                        {
                            throw new RepeatedSKUProductException();
                        }
                    }
                }
            }
        }

        public static bool CheckPoAActive(AppcoinsUnity a)
        {
            return a.enablePOA == true ? true : false;
        }

        public static bool CheckPurchaserObject(AppcoinsUnity a)
        {
            if (a.purchaserObject == null)
            {
                return true;
            }

            return false;
        }

        internal static bool IgnoreSKU(List<AppcoinsSKU> products, 
                                       AppcoinsSKU product) {
            if (product == null || product.SKUID.Equals(""))
            {
                return true;
            }

            if (products.FindAll(
                    sku => sku.SKUID.Equals(product.SKUID)
                ).Count > 2
               )
            {
                return true;
            }

            return false;
        }
    }
}
