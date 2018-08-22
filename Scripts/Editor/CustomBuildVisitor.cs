using UnityEditor;

public interface CustomBuildVisitor
{
    public bool GenericExport(string[] scenesPath, string target_dir,
                      BuildTarget b_target, BuildOptions opt);
}