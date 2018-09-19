using UnityEngine;

using System;
using System.Collections.Generic;

namespace Aptoide.AppcoinsUnity
{
    /// <summary>
    /// Class that makes the communication between the game and the Platform
    /// (where the game is running).
    /// <list type="bullet">
    /// <item>
    /// <term>Awake</term>
    /// <description>Unity's Awake Event.</description>
    /// </item>
    /// <item>
    /// <term>SetupCommunication</term>
    /// <description>
    /// Setup components of the running Platform side.
    /// </description>
    /// </item>
    /// <item>
    /// <term>SetupReceiver</term>
    /// <description>
    /// Get an instance of the of the running Platform's object communicating 
    /// with the game.
    /// </description>
    /// </item>
    /// <item>
    /// <term>SendExceptionToReceiver</term>
    /// <description>
    /// Send Exception to the Platform's object communicating with the game.
    /// </description>
    /// </item>
    /// <item>
    /// <term>GetWalletAddress</term>
    /// <description>Wallet Address.</description>
    /// </item>
    /// <item>
    /// <term>SetupWalletAddress</term>
    /// <description>
    /// Give the Wallet Address to the Platform's object communicating with the 
    /// game.
    /// </description>
    /// </item>
    /// <item>
    /// <term>GetIAB</term>
    /// <description>Check if IAB is enabled.</description>
    /// </item>
    /// <item>
    /// <term>SetupIAB</term>
    /// <description>Setup IAB component on the Platform side.</description>
    /// </item>
    /// <item>
    /// <term>AwakeReceiver</term>
    /// <description>
    /// Game and the Platform's object communicating with the game can start the
    /// communication itself.
    /// </description>
    /// </item>
    /// <item>
    /// <term>AddSKU</term>
    /// <description>Add new SKU to skus' list.</description>
    /// </item>
    /// <item>
    /// <term>MakePurchase</term>
    /// <description>Begin transaction to buy a specific SKU.</description>
    /// </item>
    /// <item>
    /// <term>PurchaseSuccess</term>
    /// <description>
    /// Callback funciton when transaction ended successfully.
    /// </description>
    /// </item>
    /// <item>
    /// <term>PurchaseFailure</term>
    /// <description>Callback function when transaction failed.</description>
    /// </item>
    /// <item>
    /// <term>GetSKUList</term>
    /// <description>Send list with all registerd SKUs.</description>
    /// </item>
    /// <item>
    /// <term>FindSKUById</term>
    /// <description>Find specific SKU by its ID.</description>
    /// </item>
    /// <item>
    /// <term>CheckForRepeatedSKU</term>
    /// <description>
    /// Check if new SKU has the same SKUID of other SKU in skus' list.
    /// </description>
    /// </item>
    /// <item>
    /// <term>GetGameObject</term>
    /// <description>Get AppcoinsUnity game object.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Aptoide.AppcoinsUnity.ASFAppcoinsUnity has 'DontDestroyOnLoad' property, so
    /// it can be used at any scene (just include it in the first scene).
    /// </para>
    /// <para>
    /// For now only the Android Platform and Unity Editor have a visitor 
    /// (IAppcoinsUnityVisitor) to communicate with 
    /// Aptoide.AppcoinsUnity.ASFAppcoinsUnity.
    /// </para>
    /// </remarks>
    public class ASFAppcoinsUnity : MonoBehaviour
    {
        // Visitor to call depending on the platform where the game is running.
        private IAppcoinsUnityVisitor appcoinsVisitor;

        // Purchase Object defined by the user.
        [Header("Add your purchaser object here")]
        public AppcoinsPurchaser purchaserObject;
        private AppcoinsPurchaser purchaserObjChoosed;

        // Wallet address where the user want to receive appcoins.
        [Header("Your wallet address for receiving Appcoins")]
        public string address;
        private string addressChoosed;

        // Enable In-App Billing.
        [Header("Uncheck to disable Appcoins IAB")]
        public bool enableIAB = true;
        private bool IABChoosed;

        // Enable POA.
        [Header("Uncheck to disable Appcoins ADS(Proof of attention)")]
        public bool enablePOA = true;
        private bool POAChoosed;

        // Enable test transactions (Ropsten net).
        [Header("Enable debug to use testnets e.g Ropsten")]
        public bool enableDebug = true;
        private bool debugChoosed;

        // List with all registerd skus.
        private List<AppcoinsSKU> skus;

        // User can add SKUs until SetupIAB is called.
        private bool canAddSku = true;

        /// <summary>
        /// Chose IAppcoinsUnityVisitor visitor in terms of which platform is 
        /// being used to run the game. Initialize skus' list and 
        /// purchaserObject.
        /// </summary>
        /// <exception cref="Aptoide.AppcoinsUnity.PlatformNotSupportedException">
        /// Thrown when platform choosed to integrate AppcoinsUnity Plugin and 
        /// run the game is not supported.
        /// </exception>
        /// <remarks>
        /// <para>
        /// Current Platform Supported: Andoroid, Unity Editor.
        /// </para>
        /// </remarks>
        internal void Awake()
        {
            if (Application.isEditor)
            {
                appcoinsVisitor = new EditorAppcoinsUnityVisitor();
            }

            else if (Application.isMobilePlatform && 
                     Application.platform == RuntimePlatform.Android)
            {
                appcoinsVisitor = new AndroidAppcoinsUnityVisitor();
            }

            else
            {
                throw new PlatformNotSupportedException();
            }

            skus = new List<AppcoinsSKU>();

            // Set up communication with Purchaser
            CheckPurchaserObject();
            purchaserObject.Init(this);

            // Give access to ASFAppcoinsUnity prefab at all Scenes
            DontDestroyOnLoad(this.gameObject);

            // Initialize private attributes (So even ig the user change the
            // public attributes after Awake event, those changes don't
            // propagate to the private values
            purchaserObjChoosed = purchaserObject;
            addressChoosed = address;
            IABChoosed = enableIAB;
            POAChoosed = enablePOA;
            debugChoosed = enableDebug;
        }

        /// <summary>
        /// Unity Start Event. Setup communication with the receiver.
        /// </summary>
        internal void Start()
        {
            SetupCommunication();
        }

        /// <summary>
        /// Setup all communication with the Platform's side object (receiver) 
        /// that will communicate with the game. (This includes setting up 
        /// receiver, give wallet address and all SKUs in skus list (if IAB is 
        /// enabled) to receiver.
        /// </summary>
        protected void SetupCommunication()
        {
            SetupReceiver();
            SetupWalletAddress();

            if (IABChoosed)
            {
                SetupIAB();
            }

            // Awake Reveiver only when all Setup's have been done
            AwakeReceiver();
        }

        /// <summary>
        /// Instantiate receiver (Platform's side object to communicate with the
        /// game).
        /// </summary>
        private void SetupReceiver()
        {
            appcoinsVisitor.SetupReceiver(this);
        }

        /// <summary>
        /// Get wallet address.
        /// </summary>
        /// <returns>
        /// Return wallet address.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This wallet address is the the receiver wallet for IAB transactions.
        /// </para>
        /// </remarks>
        public string GetWalletAddress()
        {
            return address;
        }

        /// <summary>
        /// Pass wallet address to Platform's object communicting with the game
        /// to registered and be the receiver for IAB transactions.
        /// </summary>
        private void SetupWalletAddress()
        {
            appcoinsVisitor.SetupWalletAddress(this);
        }

        /// <summary>
        /// Check if IAB is enabled.
        /// </summary>
        /// <returns>
        /// true if IAB is enabled; false otherwise.
        /// </returns>
        public bool GetIAB()
        {
            return IABChoosed;
        }

        /// <summary>
        /// All SKU's in skus' list are passed to the platform's object 
        /// communicating with the game to be registered (this is done by 
        /// calling 'RegisterSKUs' purchaserObject method).
        /// </summary>
        /// <remarks>
        /// <para>
        /// Afeter this method being called, no more SKUs can be registered, if
        /// so CannotRegisterSKUException will be thrown.
        /// </para>
        /// </remarks>
        private void SetupIAB()
        {
            // Setup IAB in the platform being used
            appcoinsVisitor.SetupIAB(this);

            // Register all SKU's in products list before setting up IAB
            purchaserObject.RegisterSKUs();

            // No more SKU's can be registered.
            canAddSku = false;
        }

        /// <summary>
        /// Check if POA is enabled.
        /// </summary>
        /// <returns>
        /// true if POA is enabled; false otherwise.
        /// </returns>
        public bool GetPOA()
        {
            return POAChoosed;
        }

        /// <summary>
        /// Start the communication between the game and the Platform's object
        /// that is communicating with the game.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is called after 'SetupReceiver', 
        /// 'SetupWalletAddress',  and 'SetupIAB' methods.
        /// </para>
        /// </remarks>
        private void AwakeReceiver()
        {
            appcoinsVisitor.AwakeReceiver(this);
        }

        /// <summary>
        /// Try to add new SKU to skus' list. If SKU <paramref name="newSku"/> 
        /// is already in skus' list, or is not well instantiated the repective 
        /// Exception will be sent.
        /// </summary>
        /// <exception cref="Aptoide.AppcoinsUnity.NullSKUProductException">
        /// Thrown when <paramref name="newSku"/> is set to null.
        /// </exception>
        /// See <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> class that 
        /// implements Appcoins SKUs.
        /// <param name="newSku">SKU to add to skus' list.</param>
        internal void AddSKU(AppcoinsSKU newSku)
        {
            if (newSku == null)
            {
                throw new NullSKUProductException();
            }

            if (!canAddSku)
            {
                throw new CannotRegisterSKUException();
            }

            // Check if 'newSku' is a valid SKU
            newSku.CheckSKU();

            // Check if 'newSku' have a sku id different from all sku id's
            // in skus list
            CheckForRepeatedSKU(newSku);

            // Add new sku to skus' list
            skus.Add(newSku);

            // Add new sku to the platform being used
            appcoinsVisitor.AddSKU(this, newSku);
        }

        /// <summary>
        /// If SKU <paramref name="sku"/> is in skus' list start transaction 
        /// flux by passing it to the Platorm' side.
        /// </summary>
        /// <param name="sku">SKU to purchase.</param>
        /// See <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> class that 
        /// implements Appcoins SKUs.
        /// <exception cref="Aptoide.AppcoinsUnity.SkuIsNotRegistedException">
        /// Thrown when <paramref name="sku"/> is not in skus' list and can't be
        /// added because 'SetupIAB' method has already been called.
        /// </exception>
        /// <exception cref="Aptoide.AppcoinsUnity.IABIsTurnedOffException">
        /// Thrown when a purchase was tried to be made but 'enableIAB' is set
        /// to fasle.
        /// </exception>
        internal void MakePurchase(AppcoinsSKU sku)
        {
            if (IABChoosed)
            {
                if (skus.Contains(sku))
                {
                    appcoinsVisitor.MakePurchase(this, sku);
                }

                else
                {
                    throw new SkuIsNotRegistedException();
                }
            }

            else
            {
                throw new IABIsTurnedOffException();
            }
        }

        /// <summary>
        /// Callback function when some transaction ended successfully on the 
        /// Platform's side.
        /// </summary>
        /// <param name="skuId">Sku id</param>
        /// See <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> class that 
        /// implements Appcoins SKUs.
        /// <remarks>
        /// <para>
        /// purchaserObject.PurchaseSucess method is called (if user wants to 
        /// handle successful transactions).
        /// </para>
        /// </remarks>
        private void PurchaseSuccess(string skuId)
        {
            purchaserObject.PurchaseSuccess(FindSKUById(skuId));
        }

        /// <summary>
        /// Callback function when some transaction failed on the Platform's
        /// side.
        /// </summary>
        /// <param name="skuId">Sku id</param>
        /// See <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> class that 
        /// implements Appcoins SKUs.
        /// <remarks>
        /// <para>
        /// purchaserObject.PurchaseFailure method is called (if user wants to 
        /// handle failed transactions).
        /// </para>
        /// </remarks>
        private void PurchaseFailure(string skuId)
        {
            purchaserObject.PurchaseFailure(FindSKUById(skuId));
        }

        /// <summary>
        /// Get skus' list.
        /// </summary>
        /// <returns>
        /// System.Collections.Generic.List with all registered SKU's.
        /// </returns>
        public List<AppcoinsSKU> GetSKUList()
        {
            return new List<AppcoinsSKU>(skus);
        }

        /// <summary>
        /// Find SKU, in skus list, with a the specific sku id 
        /// <paramref name="skuid"/>.
        /// </summary>
        /// <returns>
        /// Return Aptoide.AppcoinsUnity.AppcoinsSKU, sku in skus list with
        /// a SKUID as the same as <paramref name="skuid"/>."
        /// </returns>
        /// See <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> class that 
        /// implements Appcoins SKUs.
        /// <param name="skuid">Sku id</param>
        public AppcoinsSKU FindSKUById(string skuid)
        {
            return skus.Find(sku => sku.GetSKUId().Equals(skuid));
        }

        /// <summary>
        /// Check if 'purchaserObject is configurated (not null).
        /// </summary>
        /// <exception cref="Aptoide.AppcoinsUnity.PurchaserObjectIsNullException">
        /// Thrown when 'purchaserObject' is null.
        /// </exception>
        private void CheckPurchaserObject()
        {
            if (purchaserObject == null)
            {
                throw new PurchaserObjectIsNullException();
            }
        }

        /// <summary>
        /// Check if new SKU <paramref name="newSKU"/> have the same SKUID of 
        /// any SKU in skus' list.
        /// </summary>
        /// <exception cref="Aptoide.AppcoinsUnity.RepeatedSKUProductException">
        /// Thrown when <paramref name="newSKU"/> have the same SKUID of another
        /// skus' list element.
        /// </exception>
        /// See <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> class that 
        /// implements Appcoins SKUs.
        /// <param name="newSKU">Appcoins SKU</param>
        private void CheckForRepeatedSKU(AppcoinsSKU newSKU)
        {
            foreach(AppcoinsSKU sku in skus)
            {
                if (sku.Equals(newSKU))
                {
                    throw new RepeatedSKUProductException();
                }
            }
        }

        /// <summary>
        /// Get Aptoide.AppcoinsUnity.AppcoinsUnity game object.
        /// </summary>
        /// <returns>
        /// UnityEngine.GameObject that contains 
        /// Aptoide.AppcoinsUnity.AppcoinUnity component.
        /// </returns>
        /// See <see cref="UnityEngine.GameObject"/>
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}