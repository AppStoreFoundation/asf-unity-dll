using UnityEngine;

using System.Collections.Generic;

namespace Aptoide.AppcoinsUnity
{
    public abstract class AppcoinsUnity : MonoBehaviour
    {
        [Header("Your wallet address for receiving Appcoins")]
        public string receivingAddress;

        [Header("Uncheck to disable Appcoins IAB")]
        public bool enableIAB = true;

        [Header("Uncheck to disable Appcoins ADS(Proof of attention)")]
        public bool enablePOA = true;

        [Header("Enable debug to use testnets e.g Ropsten")]
        public bool enableDebug = true;

        [Header("Add your purchaser object here")]
        public AppcoinsPurchaser purchaserObject;

        private List<AppcoinsSKU> products;

        protected void Awake()
        {
            CheckPurchaser();

            products = new List<AppcoinsSKU>();
            purchaserObject.Init(this);
            DontDestroyOnLoad(this.gameObject);
        }

        protected abstract void Start();
        protected abstract void SetupReceiver();
        protected abstract void SetupAddress();
        protected abstract void SetupIAB();
        internal abstract void AddSKU(AppcoinsSKU sku);
        internal abstract void MakePurchase(AppcoinsSKU sku);
        internal abstract void PurchaseSuccess(AppcoinsSKU sku);
        internal abstract void PurchaseFailure(AppcoinsSKU sku);

        public List<AppcoinsSKU> GetProductList()
        {
            return products;
        }

        private void CheckPurchaser()
        {
            if (purchaserObject == null)
            {
                throw new PurchaserObjectIsNullException();
            }
        }
    }
}