using UnityEditor;
using UnityEngine;

using Aptoide.AppcoinsUnity;

public class CustomBuildGradleProjectBuild : CustomBuildProjectBuild
{
    private readonly string mainTemplatePath;
    private string gradleMem = "1536";
    private const string gradleMemLine = "org.gradle.jvmargs=-Xmx{0}M";

    private bool gradleDebugMode = false;

    private Terminal terminal;

    public CustomBuildGradleProjectBuild()
    {
        mainTemplatePath = Application.dataPath + 
                                      "/Plugins/Android/mainTemplate.gradle";
        
        terminal = Tools.GetTerminalByOS();
    }

    private string GetGradleArgs()
    {
        string gradleArgs = "assembleDebug";

        if (EditorPrefs.GetBool("appcoins_build_release", false))
        {
            gradleArgs = "assembleRelease";
        }

        if ((gradleDebugMode = 
             EditorPrefs.GetBool("appcoins_debug_mode", false)))
        {
            gradleArgs += " --debug";
        }

        return gradleArgs;
    }

    private void TurnGradleIntoExe(BuildStage stage, string gradlePath)
    {
        // If we're not in windows we need to make sure that the gradle file has 
        // exec permission and if not, set them
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            string chmodCmd = "chmod";
            string chmodArgs = "+x '" + gradlePath + "gradle'";

            return terminal.RunCommand(stage, chmodCmd, chmodArgs, ".", false);
        }
    }

    private void ChangeGradleMem()
    {
        gradleMem = EditorPrefs.GetString("appcoins_gradle_mem", "1536");
        string[] lines = { gradleMemLine.Replace("{0}", gradleMem) };
        Tools.WriteToFile(mainTemplatePath, lines);
    }

    internal override void ProjectBuild(BuildStage stage, string projPath)
    {
        string command = Tools.FixAppPath(
            EditorPrefs.GetString("appcoins_gradle_path", ""), 
            "gradle"
        );

        TurnGradleIntoExe(command);

        string gradleArgs = GetGradleArgs();
        string path = "'" + projPath + "'";
        terminal.RunCommand(stage, command, gradleArgs, path, gradleDebugMode);

        ChangeGradleMem();
    }
}