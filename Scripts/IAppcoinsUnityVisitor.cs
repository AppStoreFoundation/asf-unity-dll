using System;

namespace Aptoide.AppcoinsUnity
{
    /// <summary>
    /// Class that instatiated an AppCoins SKU.
    /// <list type="bullet">
    /// <item>
    /// <term>SetupReceiver</term>
    /// <description>AppcoinsSKU constructor.</description>
    /// </item>
    /// <item>
    /// <term>SetupWalletAddress</term>
    /// <description>Get the name of Appcoins SKU object.</description>
    /// </item>
    /// <item>
    /// <term>SetupIAB</term>
    /// <description>Get the ID of Appcoins SKU object.</description>
    /// </item>
    /// <item>
    /// <term>AwakeReceiver</term>
    /// <description>Get the price of Appcoins SKU object.</description>
    /// </item>
    /// <item>
    /// <term>AddSKU</term>
    /// <description>
    /// Check if Appcoins SKU object is well instatiated.
    /// </description>
    /// </item>
    /// <item>
    /// <term>MakePurchase</term>
    /// <description>
    /// Check if the ID of Appcoins SKU object is valid.
    /// </description>
    /// </item>
    /// </list>
    /// </summary>
    public interface IAppcoinsUnityVisitor
    {
        /// <summary>
        /// Instantiate receiver based on choosed Platform to run the game.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        void SetupReceiver(ASFAppcoinsUnity appcoinsUnity);

        /// <summary>
        /// Give wallet address to receiver.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        void SetupWalletAddress(ASFAppcoinsUnity appcoinsUnity);

        /// <summary>
        /// Enable IAB on the receiver's side.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        void SetupIAB(ASFAppcoinsUnity appcoinsUnity);

        /// <summary>
        /// Start receiver lifecycle.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        void AwakeReceiver(ASFAppcoinsUnity appcoinsUnity);

        /// <summary>
        /// Give new SKU to receiver.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        /// <param name="newSku"> new
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> to give to receiver.
        /// </param>
        void AddSKU(ASFAppcoinsUnity appcoinsUnity, AppcoinsSKU newSku);

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
        void MakePurchase(ASFAppcoinsUnity appcoinsUnity, AppcoinsSKU sku);
    }
}