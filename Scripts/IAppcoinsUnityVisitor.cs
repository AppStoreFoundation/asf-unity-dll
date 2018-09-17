using System;

namespace Aptoide.AppcoinsUnity
{
    public interface IAppcoinsUnityVisitor
    {
        void SetupReceiver(AppcoinsUnity appcoins);

        //void SendExceptionToReceiver(Exception e);

        void SetupWalletAddress(AppcoinsUnity appcoinsUnity);

        void SetupIAB(AppcoinsUnity appcoinsUnity);

        void AwakeReceiver(AppcoinsUnity appcoinsUnity);

        void AddSKU(AppcoinsUnity appcoinsUnity, AppcoinsSKU newSku);

        void MakePurchase(AppcoinsUnity appcoinsunity, AppcoinsSKU sku);
    }
}