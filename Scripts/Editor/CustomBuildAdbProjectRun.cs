using UnityEditor;

using Aptoide.AppcoinsUnity;

public class CustomBuildAdbProjectRun : CustomBuildProjectRun
{
    private Terminal terminal;

    public CustomBuildAdbProjectRun()
    {
        terminal = Tools.GetTerminalByOS();
    }

    private string GetAdbRunArgs()
    {
        return "shell am start -n '" +
                PlayerSettings.applicationIdentifier + "/" +
                CustomBuild.mainActivityPath + "'";
    }

    internal override void ProjectRun(BuildStage stage, string path)
    {

        string command = Tools.FixAppPath(
            EditorPrefs.GetString("appcoins_adb_path", ""),
            "adb");

        string adbArgs = GetAdbRunArgs();
        string cmdPath = "'" + path + "'";

        terminal.RunCommand(stage, command, adbArgs, cmdPath, false);
    }
}