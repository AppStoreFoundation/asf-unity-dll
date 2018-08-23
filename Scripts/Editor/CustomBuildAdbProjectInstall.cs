using UnityEditor;

using Aptoide.AppcoinsUnity;

public class CustomBuildAdbProjectInstall : CustomBuildProjectInstall
{
    private Terminal terminal;

    public CustomBuildAdbProjectInstall()
    {
        terminal = Tools.GetTerminalByOS();
    }

    private string GetAdbInstallArgs()
    {
        string adbArgs = "";

        if (CustomBuild.buildRelease)
        {
            adbArgs = "-d install -r './build/outputs/apk/release/" +
                       PlayerSettings.productName + "-release.apk'";
        }

        else
        {
            adbArgs = "-d install -r './build/outputs/apk/debug/" +
                       PlayerSettings.productName + "-debug.apk'";
        }

        return adbArgs;
    }

    internal override void ProjectInstall(BuildStage stage, string path)
    {
        string command = Tools.FixAppPath(
            EditorPrefs.GetString("appcoins_adb_path", ""), 
            "adb");
        
        string adbArgs = GetAdbInstallArgs();
        string cmdPath = "'" + path + "'";

        terminal.RunCommand(stage, command, adbArgs, cmdPath, false);
    }
}