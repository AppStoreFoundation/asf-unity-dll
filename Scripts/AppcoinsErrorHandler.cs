using UnityEditor;

namespace Aptoide.AppcoinsUnity
{
    public class AppcoinsErrorHandler
    {
        public static bool HandleError(APPCOINS_ERROR err)
        {
            bool ret = true;

            switch (err)
            {
                case APPCOINS_ERROR.NONE:
                    break;

                case APPCOINS_ERROR.POA_ENABLED:
                    EditorUtility.DisplayDialog(
                        "AppCoins Unity Integration",
                        "PoA is enabled and should have started now",
                        "OK"
                    );

                    break;

                case APPCOINS_ERROR.NO_PRODUCTS:
                    EditorUtility.DisplayDialog(
                        "AppCoins Unity Integration",
                        "Warning: You have no products on AppCoinsUnity prefab products list",
                        "OK"
                    );

                    ret = false;
                    break;

                case APPCOINS_ERROR.PRODUCT_NULL:
                    EditorUtility.DisplayDialog(
                        "AppCoins Unity Integration",
                        "Warning: You have null products on AppCoinsUnity prefab products list",
                        "OK"
                    );

                    ret = false;
                    break;

                case APPCOINS_ERROR.PRODUCT_REPEATED:
                    EditorUtility.DisplayDialog(
                        "AppCoins Custom Build Error",
                        "AppcoinsUnity Prefab products list: two or more elements " +
                            "have the same SKU ID.",
                        "OK"
                    );

                    ret = false;
                    break;
            }

            return ret;
        }
    }
}
