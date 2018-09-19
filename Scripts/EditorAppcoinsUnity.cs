using UnityEngine;

using System;
using System.Collections;

namespace Aptoide.AppcoinsUnity {
    public class EditorAppcoinsUnity : MonoBehaviour
    {
        MessageHandlerGUI messageHandler;

        private string title = "AppCoins Unity Integration";
        private string ok = "Got it";
        private string testSuc = "Test Success";
        private string testFail = "Test Failure";

        AppcoinsSKU currentSku;

        internal void Setup()
        {
            gameObject.AddComponent(typeof(MessageHandlerGUI));
            messageHandler = gameObject.GetComponent<MessageHandlerGUI>();
        }

        internal void Init()
        {
            // Setup editor window
            messageHandler.InitializeWindow();
            
            // Show POA Information
            if (gameObject.GetComponent<ASFAppcoinsUnity>().GetPOA())
            {
                SetupMessage(title, "POA should have started now.", ok, null);
            }
        }

        private IEnumerator WaitFor(bool var, bool res)
        {
            while (!(var == res))
            {
                yield return null;
            }

            yield break;
        }

        internal void SetupMessage(string t, string content, string suc, 
                                   string fail)
        {
            messageHandler.ChangeContent(t, content, suc, fail);
            messageHandler.Enable();

            // Wait until 'ok' button is clicked
            StartCoroutine(WaitFor(messageHandler.enabled, true));
        }

        //method used in making purchase
        internal void MakePurchase(AppcoinsSKU sku)
        {
            currentSku = sku;

            string content = "Appcoins Unity: Simulate Purchase of " +
                sku.GetName() + " (" + sku.GetSKUId() + ").";

            messageHandler.prop.AddListener(ProcessPurchase);
            // Create window to simulate purchase
            SetupMessage(title, content, testSuc, testFail);
        }

        private void ProcessPurchase(bool purchaseResult)
        {
            messageHandler.prop.RemoveAllListeners();

            if (purchaseResult)
            {
                string content = "Purchase of " + currentSku.GetName() + " (" + 
                                  currentSku.GetSKUId() + ") finished " +
                                  "successfully.";
                SetupMessage(title, content, ok, null);

                gameObject.SendMessage("PurchaseSuccess", 
                                       currentSku.GetSKUId()
                                      );
            }

            else
            {
                string content = "Purchase of " + currentSku.GetName() + " (" +
                                  currentSku.GetSKUId() + ") failed.";
                SetupMessage(title, content, ok, null);

                gameObject.SendMessage("PurchaseFailure", currentSku.GetSKUId());
            }
        }
    }
} //namespace Aptoide.AppcoinsUnity