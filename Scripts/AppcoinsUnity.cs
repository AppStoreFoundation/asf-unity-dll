// created by Lukmon Agboola(Codeberg)
// Modified by Aptoide
// Note: do not change anything here as it may break the workings of the plugin 
// else you're very sure of what you're doing.
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace Aptoide.AppcoinsUnity
{
    public class StartPurchaseEvent : UnityEvent<string> {}

    public class AppcoinsUnity : MonoBehaviour
    {
        public static string POA = "POA";
        public static string DEBUG = "DEBUG";
        public static string APPCOINS_PREFAB = "APPCOINS_PREFAB";

        [Header("Your wallet address for receiving Appcoins")]
        public string receivingAddress;
        [Header("Uncheck to disable Appcoins IAB")]
        public bool enableIAB = true;
        [Header("Uncheck to disable Appcoins ADS(Proof of attention)")]
        public bool enablePOA = true;
        [Header("Enable debug to use testnets e.g Ropsten")]
        public bool enableDebug = true;
        [Header("Add all your products here")]
        public AppcoinsSku[] products;
        [Header("Add your purchaser object here")]
        public AppcoinsPurchaser purchaserObject;

        AndroidJavaClass _class;
        AndroidJavaObject instance { get { 
                return _class.GetStatic<AndroidJavaObject>("instance"); 
            } }

        public StartPurchaseEvent purchaseEvent;

        private void Awake()
        {
            purchaserObject.Init(this);

            DontDestroyOnLoad(this.gameObject);

            // Run AppcoinsChecks in Editor Mode
            purchaseEvent = new StartPurchaseEvent();
        }

        // Use this for initialization
        void Start()
        {
            if (!Application.isEditor)
            {
                //get refference to java class
                _class = new AndroidJavaClass("com.aptoide.appcoinsunity." +
                                              "UnityAppcoins");

                //setup wallet address
                _class.CallStatic("setAddress", receivingAddress);

                //Enable or disable In App Billing
                _class.CallStatic("enableIAB", enableIAB);

                //add all your skus here
                addAllSKUs();

                //start sdk
                _class.CallStatic("start");
            }
        }

        //called to add all skus specified in the inpector window.
        private void addAllSKUs()
        {
            if (!Application.isEditor)
            {
                for (int i = 0; i < products.Length; i++)
                {
                    AppcoinsSku product = products[i];
                    if (product != null)
                        _class.CallStatic("addNewSku", product.Name, 
                                          product.SKUID, product.Price);
                }
            }
        }


        //method used in making purchase
        public void makePurchase(string skuid)
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
                purchaseEvent.Invoke(skuid);
            }

            else
            {
                _class.CallStatic("makePurchase", skuid);
            }
        }

        //callback on successful purchases
        public void purchaseSuccess(string skuid)
        {
            if (purchaserObject != null)
            {
                Debug.Log("Going to call purchaseSuccess on purchaserObject " +
                          "skuid " + skuid);

                purchaserObject.purchaseSuccess(skuid);
            }
            else
            {
                Debug.Log("purchaserObject is null");
            }
        }

        //callback on failed purchases
        public void purchaseFailure(string skuid)
        {
            if (purchaserObject != null)
            {
                Debug.Log("Going to call purchaseFailure on purchaserObject " +
                          "skuid " + skuid);

                purchaserObject.purchaseFailure(skuid);
            }
            else
            {
                Debug.Log("purchaserObject is null");
            }
        }
    }
} //namespace Aptoide.AppcoinsUnity
