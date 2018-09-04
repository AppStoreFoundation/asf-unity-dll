//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Note: do not change anything here as it may break the workings of the plugin else you're very sure of what you're doing.

using UnityEngine;
using UnityEditor;

namespace Aptoide.AppcoinsUnity
{

    public class AppcoinsUnityTests : MonoBehaviour
    {
        AppcoinsUnity appcoinsUnity;

        // Use this for initialization
        void Start()
        {
            if (Application.isEditor) {
                appcoinsUnity = GetComponent<AppcoinsUnity>();

                appcoinsUnity.onStartPurchase.AddListener(makePurchase);

                if(AppcoinsChecks.CheckPoAActive(appcoinsUnity.enablePOA))
                {
                    EditorUtility.DisplayDialog("AppCoins Unity Integration",
                        "PoA is enabled and should have started now", "OK");
                }

                AppcoinsChecks.DefaultFullCheck(appcoinsUnity.products);
            }   
        }

        //method used in making purchase
        public void makePurchase(string skuid)
        {
            if (Application.isEditor)
            {
                if (!appcoinsUnity.enableIAB)
                {
                    Debug.LogWarning("Tried to make a purchase but enableIAB is false! Please set it to true on AppcoinsUnity object before using this functionality");
                    return;
                }
                else
                {
                    if (EditorUtility.DisplayDialog("AppCoins Unity Integration", "AppCoins IAB Successfully integrated", "Test success", "Test failure"))
                    {
                        purchaseSuccess(skuid);
                    }
                    else
                    {
                        purchaseFailure(skuid);
                    }
                }
            }
        }

        //callback on successful purchases
        public void purchaseSuccess(string skuid)
        {
            if (Application.isEditor)
            {
                appcoinsUnity.purchaseSuccess(skuid);
                EditorUtility.DisplayDialog("AppCoins Unity Integration", "Purchase Success!", "OK");
            }
        }

        //callback on failed purchases
        public void purchaseFailure(string skuid)
        {
            if (Application.isEditor)
            {
                appcoinsUnity.purchaseFailure(skuid);
                EditorUtility.DisplayDialog("AppCoins Unity Integration", "Purchase Failed!", "OK");
            }
        }

    }
} //namespace Aptoide.AppcoinsUnity
