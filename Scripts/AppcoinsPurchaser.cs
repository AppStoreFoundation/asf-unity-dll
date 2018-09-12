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

        public virtual void PurchaseTest(string sku)
        {

        }

        public virtual void PurchaseSuccess(string sku)
        {
                
        }

        public virtual void PurchaseFailure(string sku)
        {
                
        }

		public void MakePurchase(string sku)
        {
			appcoinsUnity.MakePurchase (sku);
		}  

        public void AddSKU(AppcoinsSKU newProduct)
        {
            appcoinsUnity.AddSKU(newProduct);
        }

        public abstract void RegisterSKUs();
	}
} //namespace Aptoide.AppcoinsUnity
