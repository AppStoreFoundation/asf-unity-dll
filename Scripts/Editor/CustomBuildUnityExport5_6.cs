using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomBuildUnityExport5_6 : CustomBuildUnityExport
{
    private string rightDllLoc;
    private string tempDllLoc;

    public CustomBuildUnityExport5_6()
    {
        rightDllLoc = Application.dataPath + "/AppcoinsUnity/Scripts/" +
                                 "AppCoinsUnityPluginTests5_6.dll";

        tempDllLoc = Directory.GetParent(Application.dataPath).FullName +
                              "/AppCoinsUnityPluginTests5_6.dll";
    }

    internal override void UnityExport(string[] scenesPath,
                                         string target_dir,
                                         BuildTarget build_target,
                                         BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // Remove AppcoinsUnityTests.dll from the project
        File.Move(rightDllLoc, tempDllLoc);
        AssetDatabase.Refresh();

        string s = BuildPipeline.BuildPlayer(scenesPath, target_dir, b_target,
                                             opt);

        // Add AppcoinsUnityTests.dll to the project
        File.Move(tempDllLoc, rightDllLoc);
        AssetDatabase.Refresh();

        // If Export failed 's' contains something.
        if (!s.Equals(""))
        {
            throw new ExportProjectFailedException();
        }
    }
}