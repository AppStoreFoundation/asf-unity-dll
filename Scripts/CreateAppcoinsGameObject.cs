using UnityEngine;

namespace Aptoide.AppcoinsUnity
{
    class CreateAppcoinsGameObject : MonoBehaviour
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

        private void Awake()
        {
            if (Application.isEditor)
            {
                gameObject.AddComponent(typeof(EditorAppcoinsUnity));
                GetComponent<EditorAppcoinsUnity>().Init(receivingAddress, 
                                                       enableIAB, enablePOA, 
                                                       enableDebug);
            }

            else if(Application.isMobilePlatform && 
                    Application.platform == RuntimePlatform.Android
                   )
            {
                gameObject.AddComponent(typeof(AndroidAppcoinsUnity));
                GetComponent<AndroidAppcoinsUnity>().Init(receivingAddress,
                                                       enableIAB, enablePOA,
                                                       enableDebug);
            }
        }
    }
}