using UnityEditor;

using System;

namespace Aptoide.AppcoinsUnity
{
    public static class AppcoinsErrorHandler
    {
        public static void HandleError(Exception exc)
        {
            EditorUtility.DisplayDialog(
                "AppCoins Unity Integration",
                exc.Message,
                "OK"
            );
        }
    }
}
