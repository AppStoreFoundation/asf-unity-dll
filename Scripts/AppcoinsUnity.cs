// created by Lukmon Agboola(Codeberg)
// Modified by Aptoide
// Note: do not change anything here as it may break the workings of the plugin 
// else you're very sure of what you're doing.
using UnityEngine;

using System.Collections.Generic;

namespace Aptoide.AppcoinsUnity
{
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
        private List<AppcoinsSKU> products;
        [Header("Add your purchaser object here")]
        public AppcoinsPurchaser purchaserObject;

        AndroidJavaClass _class;
        AndroidJavaObject instance { get { 
                return _class.GetStatic<AndroidJavaObject>("instance"); 
            } }

        private AppcoinsUnityEditorMode appcoinsEditorMode;

        private void Awake()
        {
            products = new List<AppcoinsSKU>();

            if (purchaserObject != null)
            {
                purchaserObject.Init(this);
            }

            DontDestroyOnLoad(this.gameObject);
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

                // MessageHandlerGUI is meant to just run in editor mode 
                Destroy(GetComponent<MessageHandlerGUI>());
            }

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
