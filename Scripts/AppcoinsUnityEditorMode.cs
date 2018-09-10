//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Note: do not change anything here as it may break the workings of the plugin else you're very sure of what you're doing.

using UnityEngine.Events;

using System;

namespace Aptoide.AppcoinsUnity
{

    public class AppcoinsUnityEditorMode
    {
        AppcoinsUnity appcoinsUnity;
        MessageHandlerGUI messHandler;

        string skuID;

        private const string title = "AppCoins Unity Integration";
        private const string ok = "Got it";
        private const string testSuc = "Test Success";
        private const string testFail = "Test Failure";

        // Use this for initialization
        internal AppcoinsUnityEditorMode(AppcoinsUnity a, MessageHandlerGUI mH)
        {
            appcoinsUnity = a;
            messHandler = mH;
            messHandler.ChangeTitle(title);
        }

        internal void Start(bool a)
        {
            CheckPurchaserObject();
        }

        private void CheckPurchaserObject()
        {
            if (appcoinsUnity.purchaserObject == null)
            {
                string mess = "ASFAppcoinsUnity purchaserObject is set to null";
                SetupMessHandler(mess, ok, null, CheckPOA);
            }

            else
            {
                CheckPOA(false);
            }
        }

        internal void CheckPOA(bool a)
        {
            messHandler.prop.RemoveAllListeners();

            if (AppcoinsChecks.CheckPoAActive(appcoinsUnity))
            {
                string mess = "PoA is enabled and should have started now";
                SetupMessHandler(mess, ok, null, CheckProducts);
            }

            else
            {
                CheckProducts(false);
            }
        }

        internal void CheckProducts(bool a)
        {
            messHandler.prop.RemoveAllListeners();

            try
            {
                AppcoinsChecks.DefaultFullCheck(appcoinsUnity.products);
            }
            catch (Exception e)
            {
                SetupMessHandler(e.Message, ok, null, StopEditor);
            }
        }   

        private void SetupMessHandler(string mess, string succ, string fail, 
                                      UnityAction<bool> func)
        {
            messHandler.ChangeContent(mess, succ, fail);

            if (func != null)
            {
                messHandler.prop.AddListener(func);
            }

            messHandler.Enable();
        }

        //method used in making purchase
        internal void MakePurchase(string skuid)
        {
            skuID = skuid;

            string mess ="AppCoins IAB Successfully integrated";
            SetupMessHandler(mess, testSuc, testFail, BeginPurchase);
        }

        internal void BeginPurchase(bool test)
        {
            if (test)
            {
                purchaseSuccess(skuID);
            }

            else
            {
                purchaseFailure(skuID);
            }

            messHandler.prop.RemoveListener(BeginPurchase);
        }

        //callback on successful purchases
        public void purchaseSuccess(string skuid)
        {
            string mess = "Purchase Success!";

            appcoinsUnity.purchaseSuccess(skuid);
            SetupMessHandler(mess, ok, null, null);
        }

        //callback on failed purchases
        public void purchaseFailure(string skuid)
        {
            string mess = "Purchase Failed!";

            appcoinsUnity.purchaseFailure(skuid);
            SetupMessHandler(mess, ok, null, null);
        }

        private void StopEditor(bool a)
        {
            messHandler.prop.RemoveAllListeners();
        }
    }
} //namespace Aptoide.AppcoinsUnity
