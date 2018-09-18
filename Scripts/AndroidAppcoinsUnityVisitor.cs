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
        // Platform side oject to communicate with.
        private AndroidJavaClass androidClass;

        // Instance of andoridClass
        private AndroidJavaObject instance
        {
            get
            {
                return androidClass.GetStatic<AndroidJavaObject>("instance");
            }
        }

        /// <summary>
        /// Instantiate java class to be the receiver in an Andorid enviornment.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void SetupReceiver(ASFAppcoinsUnity appcoinsUnity)
        {
            //get refference to java class
            androidClass = 
                new AndroidJavaClass("com.aptoide.appcoinsunity.UnityAppcoins");
        }

        /// <summary>
        /// Give wallet address to receiver.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void SetupWalletAddress(ASFAppcoinsUnity appcoinsUnity)
        {
            //setup wallet address
            androidClass.CallStatic("setAddress", 
                                    appcoinsUnity.GetWalletAddress()
                                   );
        }

        /// <summary>
        /// Enable IAB on the receiver's side.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void SetupIAB(ASFAppcoinsUnity appcoinsUnity)
        {
            //Enable or disable In App Billing
            androidClass.CallStatic("enableIAB", appcoinsUnity.GetIAB());
        }

        /// <summary>
        /// Start receiver lifecycle (Initialize EditorAppcoinsUnity).
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void AwakeReceiver(ASFAppcoinsUnity appcoinsUnity)
        {
            //start sdk
            androidClass.CallStatic("start");
        }

        /// <summary>
        /// Give new SKU to receiver.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        /// <param name="newSku"> new
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> to give to receiver.
        /// </param>
        public void AddSKU(ASFAppcoinsUnity appcoinsUnity, AppcoinsSKU newSku)
        {
            //Add SKU to java class
            androidClass.CallStatic("addNewSku", newSku.GetName(),
                                  newSku.GetSKUId(), newSku.GetPrice());
        }

        /// <summary>
        /// Pass SKU to receiver to start the transaction.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        /// <param name="sku">
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> 
        /// to be purchased.
        /// </param>
        public void MakePurchase(ASFAppcoinsUnity appcoinsUnity, 
                                 AppcoinsSKU sku)
        {
            androidClass.CallStatic("makePurchase", sku.GetSKUId());
        }
    }
} //namespace Aptoide.AppcoinsUnity
