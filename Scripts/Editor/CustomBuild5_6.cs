using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Aptoide.AppcoinsUnity;

public class CustomBuildMenuItem_5_6 : CustomBuildMenuItem
{
    [MenuItem("AppCoins/Custom Android Build")]
    public static void CallAndroidCustomBuild()
    {
        CustomBuildMenuItem_5_6 c = 
                    ScriptableObject.CreateInstance<CustomBuildMenuItem_5_6>();
        c.AndroidCustomBuild();

    }

    private void AndroidCustomBuild()
    {
        //Make sure settings are correctly applied
        if (Setup())
        {
            CustomBuild buildObj = new CustomBuild_5_6();
            buildObj.ExecuteCustomBuild("android");
        }

        else
        {
            UnityEngine.Debug.LogError("Custom Build aborted.");
        }     
    }
}

public class CustomBuild_5_6 : CustomBuild
{
    protected override string GenericBuild(string[] scenesPath, string target_dir, 
                                           BuildTarget build_target, BuildOptions build_options)
    {
        string path = this.SelectPath();

        if (path == null || path.Length == 0)
        {
            return null;
        }

        string projPath = CustomBuild.GetProjectPath();

        if (path == projPath)
        {
            EditorUtility.DisplayDialog("Custom Build", "Please pick a folder that is not the project root", "Got it");
            return null;
        }

        this.DeleteIfFolderAlreadyExists(path);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        string parent = Directory.GetParent(Application.dataPath).FullName;
        File.Move(Application.dataPath + "/AppcoinsUnity/Scripts/AppCoinsUnityPluginTests5_6.dll", 
                  parent + "/AppCoinsUnityPluginTests5_6.dll");
        AssetDatabase.Refresh();

        string s =  BuildPipeline.BuildPlayer(scenesPath, path, build_target, build_options);

        File.Move(parent + "/AppCoinsUnityPluginTests5_6.dll", 
                  Application.dataPath + "/AppcoinsUnity/Scripts/AppCoinsUnityPluginTests5_6.dll");

        AssetDatabase.Refresh();

        // If Export is done succesfully s is: "".
        if (!s.Equals(""))
        {
            path = null;  // Custom Build is cancelled if path is null.
        }

        return path;
    }
}