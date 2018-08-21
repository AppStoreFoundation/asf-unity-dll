using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Aptoide.AppcoinsUnity;


public abstract class CustomBuildMenuItem : EditorWindow
{
    public const string DEFAULT_UNITY_PACKAGE_IDENTIFIER = "com.Company.ProductName";

    private AppcoinsUnity appCoinsPrefabObject = null;

    //[MenuItem("AppCoins/Setup")]
    public bool Setup() {

        ValidatePrefabName();

        if (appCoinsPrefabObject != null)
        {
            APPCOINS_ERROR error;
            error = AppCoinsChecks.CheckSKUs(appCoinsPrefabObject.products);
            if (!AppcoinsErrorHandler.HandleError(error)) { return false; }

            error = AppCoinsChecks.CheckForRepeatedSkuId(appCoinsPrefabObject.products);
            if (!AppcoinsErrorHandler.HandleError(error)) { return false; }
        }

        //Check if the active platform is Android. If it isn't change it
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        //Check if min sdk version is lower than 21. If it is, set it to 21
        if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel21)
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;

        //Check if the bunde id is the default one and change it if it to avoid that error
        if (PlayerSettings.applicationIdentifier.Equals(DEFAULT_UNITY_PACKAGE_IDENTIFIER))
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.aptoide.appcoins");

        //Make sure that gradle is the selected build system
        if (EditorUserBuildSettings.androidBuildSystem != AndroidBuildSystem.Gradle)
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        
        //Make sure all non relevant errors go away
        UnityEngine.Debug.ClearDeveloperConsole();

        UnityEngine.Debug.Log("Successfully integrated Appcoins Unity plugin!");
        return true;
    }

    //Makes sure that the prefab name is updated on the mainTemplat.gradle before the build process
    private void ValidatePrefabName()
    {
        var foundObjects = Resources.FindObjectsOfTypeAll<AppcoinsUnity>();

        if (foundObjects.Length == 0) {
            UnityEngine.Debug.LogError("Found no object with component AppcoinsUnity! Are you using the prefab?");
            return;
        }

        appCoinsPrefabObject = foundObjects[0];

        string line;
        ArrayList fileLines = new ArrayList();

        System.IO.StreamReader fileReader = new System.IO.StreamReader(Application.dataPath + "/Plugins/Android/mainTemplate.gradle");

        while ((line = fileReader.ReadLine()) != null)
        {
            if (line.Contains(AppcoinsUnity.APPCOINS_PREFAB))
            {
                int i = 0;
                string newLine = "";

                while (line[i].Equals("\t") || line[i].Equals(" "))
                {
                    i++;
                    newLine = string.Concat("\t", "");
                }

                newLine = string.Concat(newLine, line);

                //Erase content after last comma
                int lastComma = newLine.LastIndexOf(",");
                newLine = newLine.Substring(0, lastComma + 1);
                newLine = string.Concat(newLine, " \"" + appCoinsPrefabObject.gameObject.name + "\"");

                fileLines.Add(newLine);
            }

            else
            {
                fileLines.Add(line);
            }
        }

        fileReader.Close();

        System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(Application.dataPath + "/Plugins/Android/mainTemplate.gradle");

        foreach (string newLine in fileLines)
        {
            fileWriter.WriteLine(newLine);
        }

        fileWriter.Close();
    }
}

public abstract class CustomBuild
{
    internal static UnityEvent continueProcessEvent = new UnityEvent();

    public enum BuildStage
    {
        IDLE,
        UNITY_BUILD,
        GRADLE_EXE,
        GRADLE_BUILD,
        ADB_INSTALL,
        ADB_RUN,
        DONE,
    }

    // EditorPref.key: appcoins_gradle_path
    public static string gradlePath = null;

    private static string gradleWindowsPath = "C:\\Program Files\\Android\\Android Studio\\gradle\\gradle-4.4\\bin\\gradle";
    private static string gradleUnixPath = "/Applications/Android Studio.app/Contents/gradle/gradle-4.4/bin/";
    public static string gradleMem = "1536";
    public static string dexMem = "1024";

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

    private string _buildPath;

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
        EditorPrefs.SetString("appcoins_main_activity_path", CustomBuild.mainActivityPath);
        EditorPrefs.SetBool("appcoins_build_release", CustomBuild.buildRelease);
        EditorPrefs.SetBool("appcoins_run_adb_install", CustomBuild.runAdbInstall);
        EditorPrefs.SetBool("appcoins_run_adb_run", CustomBuild.runAdbRun);
        EditorPrefs.SetBool("appcoins_debug_mode", CustomBuild.debugMode);
        EditorPrefs.SetString("gradle_mem", CustomBuild.gradleMem);
        EditorPrefs.SetString("dex_mem", CustomBuild.dexMem);
    }

    internal static void LoadCustomBuildPrefs()
    {
        if(EditorPrefs.HasKey("appcoins_gradle_path"))
        {
            CustomBuild.gradlePath = EditorPrefs.GetString("appcoins_gradle_path", "");
        }

        if(EditorPrefs.HasKey("appcoins_adb_path"))
        {
            CustomBuild.adbPath = EditorPrefs.GetString("appcoins_adb_path", "");
        }

        if(EditorPrefs.HasKey("appcoins_main_activity_path"))
        {
            CustomBuild.mainActivityPath = EditorPrefs.GetString("appcoins_main_activity_path", "");
        }

        if(EditorPrefs.HasKey("appcoins_build_release"))
        {
            CustomBuild.buildRelease = EditorPrefs.GetBool("appcoins_build_release", false);
        }

        if(EditorPrefs.HasKey("appcoins_run_adb_install"))
        {
            CustomBuild.runAdbInstall = EditorPrefs.GetBool("appcoins_run_adb_install", false);
        }

        if(EditorPrefs.HasKey("appcoins_run_adb_run"))
        {
            CustomBuild.runAdbRun = EditorPrefs.GetBool("appcoins_run_adb_run", false);
        }

        if(EditorPrefs.HasKey("appcoins_debug_mode"))
        {
            CustomBuild.debugMode = EditorPrefs.GetBool("appcoins_debug_mode", false);
        }

        if(EditorPrefs.HasKey("gradle_mem"))
        {
            CustomBuild.gradleMem = EditorPrefs.GetString("gradle_mem", "1536");
        }

        if(EditorPrefs.HasKey("dex_mem"))
        {
            CustomBuild.dexMem = EditorPrefs.GetString("dex_mem", "1024");
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
                    this.ExportAndBuildCustomBuildTarget(target, scenesPath);
                }
            );
        }

        else
        {
            return;
        }
    }

    protected void ExportAndBuildCustomBuildTarget(string target, string[] scenesPath)
    {
    }

    protected void ExportAndBuildCustomBuildAndroid(string[] scenesPath)
    {
        _buildPath = null;

        // List with all the errors (when a stage is completed that 
        // error is erased from the list).
        List<CustomBuildErrors> errors = Enum
                                            .GetValue(typeof(CustomBuildErrors))
                                            .Cast<CustomBuildErrors>
                                            .ToList();

        do
        {
            // Phase 1: Export Project
            StateUnityBuild();

            // Set Dex memory to mainTemplate
            SetDexMem();
            continue_proc = AndroidCustomBuild(scenesPath); 

            // Phase 2: Build Exported Project
            if (continue_proc)
            {
                State
                StateGradleBuild();
                SetGradleMem(_buildPath);
                Build(_buildPath)
            }

            // Phase 3: Intall apk
            if (continue_proc)
            {
                StateAdbInstall();
                AdbInstall(_buildPath);
            }

            // Phase 4: Run apk
            if (continue_proc)
            {
                StateAdbRun();
                AdbRun();
            }
            else
            {
                StateBuildFailed("Error building (Unity)");
            }   
        } while (false);
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

    public void StateUnityBuild()
    {
        ChangeStage(BuildStage.UNITY_BUILD);
    }

    public void StateGradleExe()
    {
        ChangeStage(BuildStage.GRADLE_EXE);
    }

    public void StateGradleBuild()
    {
        ChangeStage(BuildStage.GRADLE_BUILD);
    }

    public void StateAdbInstall()
    {
        ChangeStage(BuildStage.ADB_INSTALL);
    }

    public void StateAdbRun()
    {
        ChangeStage(BuildStage.ADB_RUN);
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

    protected void SetGradleMem(string projPath)
    {
        StreamWriter writer = new StreamWriter(projPath + "/gradle.properties", false);
        writer.WriteLine("org.gradle.jvmargs=-Xmx" + gradleMem.ToString() + "M");
        writer.Close();
    }

    protected void SetDexMem()
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/Plugins/Android/mainTemplate.gradle");
        List<string> lines = new List<string>();

        const string strToSearch = "javaMaxHeapSize";
        string line;
        bool afterDex = false;

        while ((line = reader.ReadLine()) != null)
        {
            if (line.Contains("dexOptions"))
            {
                afterDex = true;
            }

            if (afterDex && line.Contains(strToSearch))
            {
                int tabs = line.IndexOf('j');
                line = string.Concat(line.Substring(0, tabs), "javaMaxHeapSize \"" + 
                                                      (Int32.Parse(dexMem) / 1024).ToString() +
                                                      "g\"");
                afterDex = false;
            }

            lines.Add(line);
        }

        reader.Close();

        StreamWriter writer = new StreamWriter(Application.dataPath + "/Plugins/Android/mainTemplate.gradle", false);

        foreach (string l in lines)
        {
            writer.WriteLine(l);
        }

        writer.Close();
    }

    protected string AndroidCustomBuild(string[] scenesPath)
    {
        return GenericBuild(scenesPath, null, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    public static string GetProjectPath()
    {
        string projPath = Application.dataPath;

        int index = projPath.LastIndexOf('/');
        projPath = projPath.Substring(0, index);

        return projPath;
    }

    protected abstract string GenericBuild(string[] scenesPath, string target_dir,
                                           BuildTarget build_target, BuildOptions build_options);

    protected string SelectPath()
    {
        return EditorUtility.SaveFolderPanel("Save Android Project to folder", "", "");
    }

    // If folder already exists in the chosen directory delete it.
    protected void DeleteIfFolderAlreadyExists(string path)
    {
        string[] folders = Directory.GetDirectories(path);

        for (int i = 0; i < folders.Length; i++)
        {
            if ((new DirectoryInfo(folders[i]).Name) == PlayerSettings.productName)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folders[i]);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
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

        if(CustomBuild.debugMode)
        {
            gradleArgs += " --debug";
        }

        return gradleArgs;
    }

    protected string GetAdbInstallArgs()
    {
        string adbArgs = "";

        if(CustomBuild.buildRelease)
        {
            adbArgs = "-d install -r './build/outputs/apk/release/" + PlayerSettings.productName + "-release.apk'";
        }

        else
        {
            adbArgs = "-d install -r './build/outputs/apk/debug/" + PlayerSettings.productName + "-debug.apk'";
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
