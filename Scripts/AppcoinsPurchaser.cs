//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Inherit this class to create your own purchaser class, see the example scene for more info

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aptoide.AppcoinsUnity {

	public abstract class AppcoinsPurchaser : MonoBehaviour 
    {

		AppcoinsUnity appcoinsUnity;

        public void Init(AppcoinsUnity appcoinsUnityRef)
        {
            //get refference to AppcoinsUnity class
            appcoinsUnity = appcoinsUnityRef;
		}

        public virtual void PurchaseSuccess(AppcoinsSKU sku)
        {
                
        }

        public virtual void PurchaseFailure(AppcoinsSKU sku)
        {
                
        }

		public void MakePurchase(AppcoinsSKU sku)
        {
			appcoinsUnity.MakePurchase(sku);
		}  

        public void AddSKU(AppcoinsSKU newSku)
        {
            appcoinsUnity.AddSKU(newSku);
        }

        public abstract void RegisterSKUs();
	}
} //namespace Aptoide.AppcoinsUnity
