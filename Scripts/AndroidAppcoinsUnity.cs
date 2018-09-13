// created by Lukmon Agboola(Codeberg)
// Modified by Aptoide
// Note: do not change anything here as it may break the workings of the plugin 
// else you're very sure of what you're doing.
using UnityEngine;

using System.Collections.Generic;

namespace Aptoide.AppcoinsUnity
{
    public class AndoridAppcoinsUnity: AppcoinsUnity
    {
        private AndroidJavaClass androidClass;
        private AndroidJavaObject instance
        {
            get
            {
                return androidClass.GetStatic<AndroidJavaObject>("instance");
            }
        }

        // Use this for initialization
        protected override void Start()
        {
            SetupReceiver();

            SetupAddress();

            //Enable or disable In App Billing
            androidClass.CallStatic("enableIAB", enableIAB);

            SetupIAB();

            if (Application.isEditor)
            {
                appcoinsEditorMode =
                    new AppcoinsUnityEditorMode(
                        this,
                        GetComponent<MessageHandlerGUI>()
                    );

                appcoinsEditorMode.Start(false);
            }

        }

        protected override void SetupReceiver()
        {
            //get refference to java class
            androidClass = 
                new AndroidJavaClass("com.aptoide.appcoinsunity.UnityAppcoins");
        }

        private override void SetupAddress()
        {
            //setup wallet address
            androidClass.CallStatic("setAddress", receivingAddress);
        }

        internal void SetupIAB()
        {
            if (purchaserObject != null)
            {
                purchaserObject.RegisterSKUs();
            }

            if (!Application.isEditor)
            {
                //start sdk
                _class.CallStatic("start");
            }
        }

        public List<AppcoinsSKU> GetProductList()
        {
            return products;
        }

        internal void AddSKU(AppcoinsSKU newProduct)
        {
            products.Add(newProduct);
            AddSKUToJava(newProduct);
        }

        private void AddSKUToJava(AppcoinsSKU newProduct)
        {
            if (!Application.isEditor)
            {
                _class.CallStatic("addNewSku", newProduct.Name,
                                  newProduct.SKUID, newProduct.Price);
            }
        }


        //method used in making purchase
        public void MakePurchase(string skuid)
        {
            if (!enableIAB)
            {
                Debug.LogWarning("Tried to make a purchase but enableIAB is " +
                                 "false! Please set it to true on " +
                                 "AppcoinsUnity object before using this " +
                                 "functionality");
                return;
            }

            if (Application.isEditor)
            {
                appcoinsEditorMode.MakePurchase(skuid);
            }

            else
            {
                _class.CallStatic("makePurchase", skuid);
            }
        }

        //callback on successful purchases
        public void PurchaseSuccess(string skuid)
        {
            if (purchaserObject != null)
            {
                Debug.Log("Going to call purchaseSuccess on purchaserObject " +
                          "skuid " + skuid);

                purchaserObject.PurchaseSuccess(skuid);
            }
            else
            {
                Debug.Log("purchaserObject is null");
            }
        }

        //callback on failed purchases
        public void PurchaseFailure(string skuid)
        {
            if (purchaserObject != null)
            {
                Debug.Log("Going to call purchaseFailure on purchaserObject " +
                          "skuid " + skuid);

                purchaserObject.PurchaseFailure(skuid);
            }
            else
            {
                Debug.Log("purchaserObject is null");
            }
        }

        private AppcoinsSKU FindSKUById(string skuid)
        {
            return products.Find(sku => sku.SKUID.Equals(skuid));
        }
    }
} //namespace Aptoide.AppcoinsUnity
