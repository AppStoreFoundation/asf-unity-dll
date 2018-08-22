using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections.Generic;

using Aptoide.AppcoinsUnity;


public abstract class CustomBuildMenuItem : EditorWindow
{
    private AppcoinsUnity appCoinsPrefabObject = null;

    protected abstract void PlatformSetup();


}

public abstract class CustomBuild
{
    internal static UnityEvent continueProcessEvent = new UnityEvent();

    public enum BuildStage
    {
        IDLE,
        UNITY_EXPORT,
        PROJECT_BUILD,
        PROJECT_INSTALL,
        PROJECT_RUN,
        DONE,
    }

    // Defualt package identifier
    protected const string DEFAULT_UNITY_PACKAGE_IDENTIFIER = "com.Company.ProductName";

    // EditorPref.key: appcoins_gradle_path
    public static string gradlePath = null;

    private static string gradleWindowsPath = "C:\\Program Files\\Android\\" +
        "Android Studio\\gradle\\gradle-4.4\\bin\\gradle";
    
    private static string gradleUnixPath = "/Applications/Android Studio.app/" +
        "Contents/gradle/gradle-4.4/bin/";
    
    // EditorPref.key: appcoins_adb_path
    public static string adbPath = EditorPrefs.GetString("AndroidSdkRoot") + "/platform-tools/adb";

    // EditorPref.key: appcoins_run_adb_install
    public static bool runAdbInstall = false;

    // EditorPref.key: appcoins_run_adb_run
    public static bool runAdbRun = false;

    // EditorPref.key: appcoins_build_release
    public static bool buildRelease = false;

    // EditorPref.key: appcoins_debug_mode
    public static bool debugMode = false;

    // public static string mainActivityPath = "com.unity3d.player.UnityPlayerActivity";
    // EditorPref.key: appcoins_main_activity_path
    public static string mainActivityPath = PlayerSettings.applicationIdentifier + ".UnityPlayerActivity";

    public static BuildStage stage;

    protected string ANDROID_STRING = "android";
    protected string BASH_LOCATION = "/bin/bash";
    protected string CMD_LOCATION = "cmd.exe";
    private string TERMINAL_CHOSEN = null;

    protected string _buildPath;

    CustomBuildVisitor vis = new CustomBuildVisitor_5_6();

    public CustomBuild()
    {
        StateBuildIdle();
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            TERMINAL_CHOSEN = BASH_LOCATION;
            gradlePath = gradleUnixPath;
        }

        else if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            TERMINAL_CHOSEN = CMD_LOCATION;
            gradlePath = gradleWindowsPath;
        }

        else
        {
            UnityEngine.Debug.LogError("Please run Unity on a desktop OS");
        }

        CustomBuild.LoadCustomBuildPrefs();
    }

    internal static void SetCustomBuildPrefs()
    {
        EditorPrefs.SetString("appcoins_gradle_path", CustomBuild.gradlePath);
        EditorPrefs.SetString("appcoins_adb_path", CustomBuild.adbPath);
        EditorPrefs.SetString("appcoins_main_activity_path", 
                              CustomBuild.mainActivityPath);
        
        EditorPrefs.SetBool("appcoins_build_release", CustomBuild.buildRelease);
        EditorPrefs.SetBool("appcoins_run_adb_install", 
                            CustomBuild.runAdbInstall);
        
        EditorPrefs.SetBool("appcoins_run_adb_run", CustomBuild.runAdbRun);
        EditorPrefs.SetBool("appcoins_debug_mode", CustomBuild.debugMode);
        EditorPrefs.SetString("appcoins_gradle_mem", CustomBuild.gradleMem);
        EditorPrefs.SetString("appconis_dex_mem", CustomBuild.dexMem);
    }

    internal static void LoadCustomBuildPrefs()
    {
        if(EditorPrefs.HasKey("appcoins_gradle_path"))
        {
            CustomBuild.gradlePath = EditorPrefs.GetString(
                "appcoins_gradle_path", "");
        }

        if(EditorPrefs.HasKey("appcoins_adb_path"))
        {
            CustomBuild.adbPath = EditorPrefs.GetString(
                "appcoins_adb_path", "");
        }

        if(EditorPrefs.HasKey("appcoins_main_activity_path"))
        {
            CustomBuild.mainActivityPath = EditorPrefs.GetString(
                "appcoins_main_activity_path", "");
        }

        if(EditorPrefs.HasKey("appcoins_build_release"))
        {
            CustomBuild.buildRelease = EditorPrefs.GetBool(
                "appcoins_build_release", false);
        }

        if(EditorPrefs.HasKey("appcoins_run_adb_install"))
        {
            CustomBuild.runAdbInstall = EditorPrefs.GetBool(
                "appcoins_run_adb_install", false);
        }

        if(EditorPrefs.HasKey("appcoins_run_adb_run"))
        {
            CustomBuild.runAdbRun = EditorPrefs.GetBool(
                "appcoins_run_adb_run", false);
        }

        if(EditorPrefs.HasKey("appcoins_debug_mode"))
        {
            CustomBuild.debugMode = EditorPrefs.GetBool(
                "appcoins_debug_mode", false);
        }

        if(EditorPrefs.HasKey("appcoins_gradle_mem"))
        {
            CustomBuild.gradleMem = EditorPrefs.GetString("appcoins_gradle_mem",
                                                          "1536");
        }

        if(EditorPrefs.HasKey("appcoins_dex_mem"))
        {
            CustomBuild.dexMem = EditorPrefs.GetString("appcoins_dex_mem", 
                                                       "1024");
        }
    }

    public void ExecuteCustomBuild(string target)
    {
        if (TERMINAL_CHOSEN != null)
        {
            ExportScenes expScenes = new ExportScenes();
            expScenes.AllScenesToExport();
            CustomBuild.continueProcessEvent.RemoveAllListeners();
            CustomBuild.continueProcessEvent.AddListener(
                delegate
                {
                    string[] scenesPath = expScenes.ScenesToString();
                    RunCustomBuild(target, scenesPath);
                }
            );
        }

        else
        {
            return;
        }
    }

    // Run all custom build phases
    protected virtual void RunCustomBuild(string target, string[] scenesPath)
    {
        _buildPath = null;

        // Phase 1: Export Project
        StateUnityExport();
        ExportProject(vis, scenesPath);

        // Phase 2: Build Exported Project
        StateProjectBuild();
        BuildProject(_buildPath);

        // Phase 3: Intall apk
        StateProjectInstall();
        InstallProject(_buildPath);

        // Phase 4: Run apk
        StateProjectRun();
        RunProject();
    }

    protected abstract void ExportProject(CustomBuildVisitor vis,
                                          string[] scenesPath);

    protected abstract void BuildProject(string projPath);

    protected abstract void InstallProject(string projPath);

    protected abstract void RunProject();

    protected void SelectProjectPath()
    {
        _buildPath = SelectPath();

        if (_buildPath == null || _buildPath.Length == 0)
        {
            throw new ExportProjectPathIsNullException();
        }

        string projPath = CustomBuild.GetProjectPath();
        if (_buildPath == projPath)
        {
            throw new ExportProjectPathIsEqualToUnityProjectPathException();
        }

        DeleteIfFolderAlreadyExists(_buildPath);
    }

    #region State Handling

    private void ChangeStage(BuildStage theStage)
    {
        stage = theStage;
    }

    public void StateBuildIdle()
    {
        ChangeStage(BuildStage.IDLE);
    }

    public void StateUnityExport()
    {
        ChangeStage(BuildStage.UNITY_EXPORT);
    }

    public void StateProjectBuild()
    {
        ChangeStage(BuildStage.PROJECT_BUILD);
    }

    public void StateProjectInstall()
    {
        ChangeStage(BuildStage.PROJECT_INSTALL);
    }

    public void StateProjectRun()
    {
        ChangeStage(BuildStage.PROJECT_RUN);
    }

    public void StateBuildDone()
    {
        ChangeStage(BuildStage.DONE);

        if (_buildPath != null)
            EditorUtility.DisplayDialog("Custom Build", "Build Done!", "OK");
    }

    public void StateBuildFailed(string errorMsg)
    {
        ChangeStage(BuildStage.IDLE);

        EditorUtility.DisplayDialog("Custom Build", "Build Failed!\n" + errorMsg, "OK");
    }

    #endregion

    protected string SelectPath()
    {
        return EditorUtility.SaveFolderPanel(
            "Save Android Project to folder", 
            "", 
            ""
        );
    }
}
