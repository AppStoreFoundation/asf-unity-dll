//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Note: do not change anything here as it may break the workings of the plugin else you're very sure of what you're doing.

using UnityEngine;

namespace Aptoide.AppcoinsUnity
{

    public class AppcoinsUnityEditorMode : MonoBehaviour
    {
        AppcoinsUnity appcoinsUnity;
        MessageHandler messHandler;

        private const string title = "AppCoins Unity Integration";
        private const string ok = "Got it";
        private const string testSuc = "Test Success";
        private const string testFail = "Test Failure";

        // Use this for initialization
        private void Start()
        {
            appcoinsUnity = GetComponent<AppcoinsUnity>();
            messHandler = new DisplayDialogMessageHandler();
            
            appcoinsUnity.purchaseEvent.AddListener(MakePurchase);

            if (AppcoinsChecks.CheckPoAActive(appcoinsUnity.enablePOA))
            {
                string mess = "PoA is enabled and should have started now";
                messHandler.HandleMessage(title, mess, ok);
            }
                
            AppcoinsChecks.DefaultFullCheck(appcoinsUnity.products);
        }   

        //method used in making purchase
        internal void MakePurchase(string skuid)
        {
            string mess = "AppCoins IAB Successfully integrated";

            if (messHandler.DualOptionWithMessage(title, mess, testFail, 
                                                  testSuc)
               )
            {
                purchaseSuccess(skuid);
            }
            else
            {
                purchaseFailure(skuid);
            }
        }

        //callback on successful purchases
        public void purchaseSuccess(string skuid)
        {
            string mess = "Purchase Success!";

            appcoinsUnity.purchaseSuccess(skuid);
            messHandler.HandleMessage(title, mess, ok);
        }

        //callback on failed purchases
        public void purchaseFailure(string skuid)
        {
            string mess = "Purchase Failed!";

            appcoinsUnity.purchaseFailure(skuid);
            messHandler.HandleMessage(title, mess, ok);
        }

    }
} //namespace Aptoide.AppcoinsUnity
