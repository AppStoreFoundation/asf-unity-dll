using System;

namespace Aptoide.AppcoinsUnity
{
    public class EditorAppcoinsUnityVisitor : IAppcoinsUnityVisitor
    {
        private EditorAppcoinsUnity editorAppc;

        public void SetupReceiver(AppcoinsUnity appcoinsUnity)
        {
            editorAppc = appcoinsUnity.GetGameObject().AddComponent(
                typeof(EditorAppcoinsUnity)
            ) as EditorAppcoinsUnity;

            editorAppc.Setup();
        }

        //public void SendExceptionToReceiver(Exception e)
        //{
        //    editorAppc.SetupMessage(e);
        //}

        public void SetupWalletAddress(AppcoinsUnity appcoinsUnity)
        {
            // Nothing to do
        }

        public void SetupIAB(AppcoinsUnity appcoinsUnity)
        {
            // Nothing to do
        }

        public void AwakeReceiver(AppcoinsUnity appcoinsUnity)
        {
            editorAppc.Init();
        }

        public void AddSKU(AppcoinsUnity appcoinsUnity, AppcoinsSKU newSku)
        {
            // Nothing to do
        }

        public void MakePurchase(AppcoinsUnity appcoinsUnity, AppcoinsSKU sku)
        {
            editorAppc.MakePurchase(sku);
        }
    }
}