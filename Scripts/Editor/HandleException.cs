using UnityEditor;
using System;

public static class ExceptionHandler
{
    public static void Handle(CustomBuildException exception)
    {
        EditorUtility.DisplayDialog("Custom Build", 
                                    exception.message, 
                                    "Got it");
    }
}