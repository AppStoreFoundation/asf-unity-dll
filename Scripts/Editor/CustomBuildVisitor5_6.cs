using UnityEditor;
using UnityEngine;
using System.IO;

public class CustomBuildVisitor_5_6 : CustomBuildVisitor
{
    private string rightDllLoc;
    private string tempDllLoc;

    public CustomBuildVisitor_5_6()
    {
        rightDllLoc = Application.dataPath + "/AppcoinsUnity/Scripts/" +
                                 "AppCoinsUnityPluginTests5_6.dll";
        
        tempDllLoc = Directory.GetParent(Application.dataPath).FullName +
                              "/AppCoinsUnityPluginTests5_6.dll";
    }

    public void GenericExport(string[] scenesPath, string target_dir,
                             BuildTarget b_target, BuildOptions opt)
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