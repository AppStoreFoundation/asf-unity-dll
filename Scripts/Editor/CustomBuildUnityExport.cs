using UnityEditor;

public abstract class CustomBuildUnityExport
{
    internal abstract void UnityExport(string[] scenesPath,
                                         string target_dir,
                                         BuildTarget build_target,
                                         BuildOptions build_options);
}