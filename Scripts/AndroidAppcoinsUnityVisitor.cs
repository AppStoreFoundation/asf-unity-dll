// created by Lukmon Agboola(Codeberg)
// Modified by Aptoide
// Note: do not change anything here as it may break the workings of the plugin 
// else you're very sure of what you're doing.
using UnityEngine;

using System.Collections.Generic;
using System;

namespace Aptoide.AppcoinsUnity
{
    public class AndroidAppcoinsUnityVisitor : IAppcoinsUnityVisitor
    {
        private AndroidJavaClass androidClass;
        private AndroidJavaObject instance
        {
            get
            {
                return androidClass.GetStatic<AndroidJavaObject>("instance");
            }
        }

        public void SetupReceiver(AppcoinsUnity appcoinsUnity)
        {
            //get refference to java class
            androidClass = 
                new AndroidJavaClass("com.aptoide.appcoinsunity.UnityAppcoins");
        }

        //public void SendExceptionToReceiver(Exception e)
        //{
        //    // Send Message to java
        //}

        public void SetupWalletAddress(AppcoinsUnity appcoinsUnity)
        {
            //setup wallet address
            androidClass.CallStatic("setAddress", 
                                    appcoinsUnity.GetWalletAddress()
                                   );
        }

        public void SetupIAB(AppcoinsUnity appcoinsUnity)
        {
            //Enable or disable In App Billing
            androidClass.CallStatic("enableIAB", appcoinsUnity.GetIAB());
        }

        public void AwakeReceiver(AppcoinsUnity appcoinsUnity)
        {
            //start sdk
            androidClass.CallStatic("start");
        }


        public void AddSKU(AppcoinsUnity appcoinsUnity, AppcoinsSKU newSku)
        {
            //Add SKU to java class
            androidClass.CallStatic("addNewSku", newSku.GetName(),
                                  newSku.GetSKUId(), newSku.GetPrice());
        }

        //method used in making purchase
        public void MakePurchase(AppcoinsUnity appcoinsUnity, AppcoinsSKU sku)
        {
            androidClass.CallStatic("makePurchase", sku.GetSKUId());
        }
    }
} //namespace Aptoide.AppcoinsUnity
