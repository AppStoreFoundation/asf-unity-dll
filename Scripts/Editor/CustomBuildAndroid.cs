using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections.Generic;

using Aptoide.AppcoinsUnity;

public class CustomBuildAndroid : CustomBuild
{
    private string expProjPath;

    internal string gradleMem = "1536";
    internal string dexMem = "1024";

    private const string gradleMemLine = "org.gradle.jvmargs=-Xmx{0}M";
    private const string dexMemLine = "javaMaxHeapSize";
    private readonly string[] dexContainer = {"dexOptions"};
    private readonly string mainTemplatePath = 
        Application.dataPath + "/Plugins/Android/mainTemplate.gradle";

    protected void PlatformSteup()
    {
        AppcoinsUnity appCoinsPrefabObj = AppCoinsChecks.ValidatePrefabName();

        if (appCoinsPrefabObj != null)
        {
            APPCOINS_ERROR error;
            error = AppCoinsChecks.CheckSKUs(appCoinsPrefabObj.products);
            if (!AppcoinsErrorHandler.HandleError(error)) { }

            error = AppCoinsChecks.CheckForRepeatedSkuId(appCoinsPrefabObj.products);
            if (!AppcoinsErrorHandler.HandleError(error)) { }
        }

        //Check if the active platform is Android. If it isn't change it
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Android, 
                BuildTarget.Android
            );

        //Check if min sdk version is lower than 21. If it is, set it to 21
        if (PlayerSettings.Android.minSdkVersion < 
                AndroidSdkVersions.AndroidApiLevel21
           )
            PlayerSettings.Android.minSdkVersion = 
                AndroidSdkVersions.AndroidApiLevel21;

        //Check if the bunde id is the default one and change it if it to 
        // avoid that error
        if (PlayerSettings.applicationIdentifier.Equals(
            DEFAULT_UNITY_PACKAGE_IDENTIFIER)
           )
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, 
                                                    "com.aptoide.appcoins");

        //Make sure that gradle is the selected build system
        if (EditorUserBuildSettings.androidBuildSystem != 
            AndroidBuildSystem.Gradle
           )
            EditorUserBuildSettings.androidBuildSystem = 
                AndroidBuildSystem.Gradle;
        
        //Make sure all non relevant errors go away
        UnityEngine.Debug.ClearDeveloperConsole();

        UnityEngine.Debug.Log("Successfully integrated Appcoins Unity plugin!");
    } 

    protected override void ExportProject(CustomBuildVisitor vis, 
                                          string[] scenesPath)
    {
        SelectProjectPath();  // To '_buildPath' property
        expProjPath = _buildPath + "/" + PlayerSettings.productName;
        ChangeDexMemory();
        vis.GenericExport(scenesPath, _buildPath, BuildTarget.Android, 
                         BuildOptions.AcceptExternalModificationsToPlayer);
    }

    protected override void BuildProject(string projPath)
    {
        expProjPath = buildPath + "/" + 
        Tools.WriteToFile()
    }

    protected override void InstallProject(string projPath)
    {
        throw new NotImplementedException();
    }

    protected override void RunProject()
    {
        throw new NotImplementedException();
    }

    private void ChangeDexMemory()
    {
        int dexMemToGB = Int32.Parse(dexMem) / 1024;
        string newDexLine = dexMemLine + " \"" + dexMemToGB.ToString() + "\"";
        Tools.ChangeLineInFile(mainTemplatePath, dexMemLine, dexContainer, 
                               newDexLine, 1);
    }

    //If path for app contains appName remove it
    protected void FixAppPath(ref string path, string AppName)
    {
        string fileName = Path.GetFileName(path);

        if (fileName == AppName)
        {
            path = Path.GetDirectoryName(path) + "/";
        }
    }

    protected string GetChmodArgs()
    {
        return "+x '" + gradlePath + "gradle'";
    }

    protected string GetGradleArgs()
    {
        string gradleArgs = "assembleDebug";

        if (CustomBuild.buildRelease)
            gradleArgs = "assembleRelease";

        if (CustomBuild.debugMode)
        {
            gradleArgs += " --debug";
        }

        return gradleArgs;
    }

    protected string GetAdbInstallArgs()
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

    protected string GetAdbRunArgs()
    {
        return "shell am start -n '" +
                PlayerSettings.applicationIdentifier + "/" +
                CustomBuild.mainActivityPath + "'";
    }

    protected bool TurnGradleIntoExe(string path)
    {
        //If we're not in windows we need to make sure that the gradle file has exec permission
        //and if not, set them
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            string cmdPath = "'" + path + "'";
            string chmodCmd = "chmod";

            return terminal.RunCommand(stage, chmodCmd, chmodArgs, ".", false);
        }
    }

    protected bool BuildProject()
    {
        FixAppPath(ref CustomBuild.gradlePath, "gradle");

        string cmdPath = "'" + path + "'";
        string gradleCmd = "'" + gradlePath + "gradle'";


        Terminal terminal = null;
        if (TERMINAL_CHOSEN == CMD_LOCATION)
        {
            terminal = new CMD();
        }

        else
        {
            terminal = new Bash();
        }

    }

    terminal.RunCommand(1, gradleCmd, gradleArgs, cmdPath, CustomBuild.debugMode, onDoneCallback);
    }

//Runs overriding ADB install process
protected bool AdbInstall()
{
    this.FixAppPath(ref CustomBuild.adbPath, "adb");

    string adbCmd = "'" + CustomBuild.adbPath + "adb'";

    string adbArgs = "";

    string cmdPath = "'" + path + "'";

    Terminal terminal = null;
    if (TERMINAL_CHOSEN == CMD_LOCATION)
    {
        terminal = new CMD();
    }

    else
    {
        terminal = new Bash();
    }

    terminal.RunCommand(2, adbCmd, adbArgs, cmdPath, false, onDoneCallback);
}

protected bool AdbRun()
{
    this.FixAppPath(ref CustomBuild.adbPath, "adb");

    string adbCmd = "'" + CustomBuild.adbPath + "adb'";


    string cmdPath = "'" + path + "/" + PlayerSettings.productName + "'";

    Terminal terminal = null;
    if (TERMINAL_CHOSEN == CMD_LOCATION)
    {
        terminal = new CMD();
    }

    else
    {
        terminal = new Bash();
    }

    terminal.RunCommand(2, adbCmd, adbArgs, cmdPath, false, onDoneCallback);
}
}