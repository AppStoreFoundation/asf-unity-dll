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
            
            // Setup listener to propagate purchase result
            messageHandler.prop.AddListener(ProcessPurchase);
        }

        private IEnumerator WaitUntilButtonClicked()
        {
            yield return new WaitUntil(() => messageHandler.isEnabled == false);
        }

        internal void SetupMessage(Exception e)
        {
            string t = "Error";
            messageHandler.ChangeContent(t, e.Message, ok, null);

            // Wait until 'ok' button is clicked
            StartCoroutine(WaitUntilButtonClicked());
        }

        //method used in making purchase
        internal void MakePurchase(AppcoinsSKU sku)
        {
            currentSku = sku;

            string content = "Appcoins Unity: Simulate Purchase of " +
                sku.GetName() + "(" + sku.GetSKUId() + ").";
            
            // Create window to simulate purchase
            messageHandler.ChangeContent(title, content, testSuc, testFail);

            StartCoroutine(WaitUntilButtonClicked());
        }

        private void ProcessPurchase(bool purchaseResult)
        {
            messageHandler.prop.RemoveAllListeners();

            if (purchaseResult)
            {
                gameObject.SendMessage("PurchaseSuccess", currentSku.GetSKUId()); 
            }

            else
            {
                gameObject.SendMessage("PurchaseFailure", currentSku.GetSKUId());
            }

            messageHandler.prop.AddListener(ProcessPurchase);
        }
    }
} //namespace Aptoide.AppcoinsUnity