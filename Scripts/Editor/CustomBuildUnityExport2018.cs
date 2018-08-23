using UnityEditor;

public class CustomBuildUnityExport2018 : CustomBuildUnityExport
{
    internal override void UnityExport(string[] scenesPath, 
                                       string target_dir, 
                                       BuildTarget build_target, 
                                       BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Android, BuildTarget.Android);

        UnityEditor.Build.Reporting.BuildReport error = 
            BuildPipeline.BuildPlayer(scenesPath, 
                                      target_dir, 
                                      build_target, 
                                      build_options);

        // Check if export failed.
        bool fail = (
            error.summary.result == 
                UnityEditor.Build.Reporting.BuildResult.Failed ||
            error.summary.result == 
                UnityEditor.Build.Reporting.BuildResult.Cancelled
        );

        if (fail)
        {
            throw new ExportProjectFailedException();
        }
    }
}