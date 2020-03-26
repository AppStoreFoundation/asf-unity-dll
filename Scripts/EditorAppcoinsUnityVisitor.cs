using System;

namespace Aptoide.AppcoinsUnity
{
    /// <summary>
    /// Concrete class to that implements 
    /// <see cref="Aptoide.AppcoinsUnity.IAppcoinsUnityVisitor"/> interface.
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
    public class EditorAppcoinsUnityVisitor : IAppcoinsUnityVisitor
    {
        // Platform side oject to communicate with.
        private EditorAppcoinsUnity editorAppc;

        /// <summary>
        /// Create EditorAppcoinsUnity Component to test ASF plugin integration.
        /// (simulaing purchases).
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void SetupReceiver(ASFAppcoinsUnity appcoinsUnity)
        {
            // Add EditorAppcoinsUnity Component to ASFAppcoinsUnity game object
            editorAppc = appcoinsUnity.GetGameObject().AddComponent(
                typeof(EditorAppcoinsUnity)
            ) as EditorAppcoinsUnity;

            // Set up EditorAppcoinsUnity Component.
            editorAppc.Setup();
        }

        /// <summary>
        /// Give wallet address to receiver.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void SetupWalletAddress(ASFAppcoinsUnity appcoinsUnity)
        {
            // Nothing to do
        }

        /// <summary>
        /// Enable IAB on the receiver's side.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void SetupIAB(ASFAppcoinsUnity appcoinsUnity)
        {
            // Nothing to do
        }

        /// <summary>
        /// Start receiver lifecycle (Initialize EditorAppcoinsUnity).
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        public void AwakeReceiver(ASFAppcoinsUnity appcoinsUnity)
        {
            editorAppc.Init();
        }

        /// <summary>
        /// Give new SKU to receiver.
        /// </summary>
        /// <param name="appcoinsUnity">
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> object.
        /// </param>
        /// <param name="newSku"> new
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> to give to receiver.
        /// </param>
        public void AddSKU(ASFAppcoinsUnity appcoinsUnity, AppcoinsSKU newSku)
        {
            // Nothing to do
        }

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
        public void MakePurchase(ASFAppcoinsUnity appcoinsUnity, 
                                 AppcoinsSKU sku)
        {
            editorAppc.MakePurchase(sku);
        }
    }
}