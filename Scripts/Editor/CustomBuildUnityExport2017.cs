using UnityEditor;

public class CustomBuildUnityExport2017 : CustomBuildUnityExport
{
    internal override void UnityExport(string[] scenesPath, 
                                         string target_dir, 
                                         BuildTarget build_target, 
                                         BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Android, BuildTarget.Android);

        string s = BuildPipeline.BuildPlayer(
            scenesPath, target_dir, build_target, build_options);

        // If Export is done succesfully s is: "".
        if (!s.Equals(""))
        {
            throw new ExportProjectFailedException();
        }
    }
}