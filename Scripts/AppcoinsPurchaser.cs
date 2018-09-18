//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Inherit this class to create your own purchaser class, see the example scene for more info

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aptoide.AppcoinsUnity {

    /// <summary>
    /// Class that instatiated an AppCoins SKU.
    /// <list type="bullet">
    /// <item>
    /// <term>Init</term>
    /// <description>Get reference to ASFAppcoinsUnity.</description>
    /// </item>
    /// <item>
    /// <term>PurchaseSuccess</term>
    /// <description>
    /// Callback function when a transaction finished successfully.
    /// </description>
    /// </item>
    /// <item>
    /// <term>PurchaseFailure</term>
    /// <description>Callback function when a transaction failed.</description>
    /// </item>
    /// <item>
    /// <term>MakePurchase</term>
    /// <description>Initiate a purchase.</description>
    /// </item>
    /// <item>
    /// <term>AddSKU</term>
    /// <description>
    /// Add new SKU to ASFAppcoinsUnity.
    /// </description>
    /// </item>
    /// <item>
    /// <term>RegisterSKUs</term>
    /// <description>
    /// Method to be called by ASFAppcoinsUnity at unity Start Event.
    /// </description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>
    /// AppcoinsSKU can be instatiated without a name.
    /// </para>
    /// </remarks>
	public abstract class AppcoinsPurchaser : MonoBehaviour 
    {
        //  Reference to ASFAppcoinsUnity Component
		ASFAppcoinsUnity appcoinsUnity;

        /// <summary>
        /// Get reference of ASFAppcoinsUnity Component
        /// </summary>
        /// <param name="appcoinsUnityRef">ASFAppcoinsUnity reference.</param>
        public void Init(ASFAppcoinsUnity appcoinsUnityRef)
        {
            //get reference to ASFAppcoinsUnity class
            appcoinsUnity = appcoinsUnityRef;
		}

        /// <summary>
        /// Callback function when a transaction finish successfully.
        /// </summary>
        /// <param name="sku">SKU purchased.</param>
        /// <remarks>
        /// <para>
        /// This method is empty and its main purpose is to the user override it
        /// with his custom implementation when a SKU is purchased.
        /// </para>
        /// </remarks>
        public virtual void PurchaseSuccess(AppcoinsSKU sku)
        {
            // Nothing to do
        }

        /// <summary>
        /// Callback function when a transaction fails.
        /// </summary>
        /// <param name="sku">SKU tryied to purchase.</param>
        /// <remarks>
        /// <para>
        /// This method is empty and its main purpose is to the user override it
        /// with his custom implementation when a purchase fails.
        /// </para>
        /// </remarks>
        public virtual void PurchaseFailure(AppcoinsSKU sku)
        {
            // Nothing to do
        }

        /// <summary>
        /// Request to ASFAppcoinsUnity to initiate a transaction of 
        /// <paramref name="sku"/>.
        /// </summary>
        /// <param name="sku">
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/>.
        /// </param>
        /// <remarks>
        /// <para>
        /// If enableIAB attribute of 
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> is false a 
        /// Exception will be thrown 
        /// <see cref="Aptoide.AppcoinsUnity.IABIsTurnedOffException"/>.
        /// </para>
        /// <para>
        /// If <paramref name="sku"/> is not registered a Exception will be 
        /// thrown <see cref="Aptoide.AppcoinsUnity.SkuIsNotRegistedException"/>.
        /// </para>
        /// </remarks>
		public void MakePurchase(AppcoinsSKU sku)
        {
			appcoinsUnity.MakePurchase(sku);
		}  

        /// <summary>
        /// Register new <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/>.
        /// </summary>
        /// <param name="newSku">New 
        /// <see cref="Aptoide.AppcoinsUnity.AppcoinsSKU"/> to register.
        /// </param>
        public void AddSKU(AppcoinsSKU newSku)
        {
            appcoinsUnity.AddSKU(newSku);
        }

        /// <summary>
        /// Method to be called by 
        /// <see cref="Aptoide.AppcoinsUnity.ASFAppcoinsUnity"/> to register all
        /// SKUs.
        /// </summary>
        /// <remarks>
        /// <para>
        /// We highly recommend add all wanted SKUs by calling 'AddSKU' method
        /// inside this method.
        /// </para>
        /// </remarks>
        public abstract void RegisterSKUs();
	}
} //namespace Aptoide.AppcoinsUnity
