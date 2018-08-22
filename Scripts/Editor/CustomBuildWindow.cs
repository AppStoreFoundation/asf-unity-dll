using UnityEditor;
using UnityEngine;

// Draw the window for the user select what scenes he wants to export
// and configure player settings.

public class CustomBuildWindow : EditorWindow
{
    public static CustomBuildWindow instance;
    public Vector2 scrollViewVector = Vector2.zero;

    //Create the custom Editor Window
    public static void CreateExportScenesWindow()
    {
        ExportScenes.buildScenesEnabled = ExportScenes.GetScenesEnabled();

        CustomBuildWindow.instance = (CustomBuildWindow)
            EditorWindow.GetWindowWithRect(
                typeof(CustomBuildWindow),
                new Rect(0, 0, 600, 500),
                true,
                "Custom Build Settings"
            );

        instance.minSize = new Vector2(600, 500);
        instance.autoRepaintOnSceneChange = true;
        instance.Show();
    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    void OnGUI()
    {
        switch (CustomBuild.stage)
        {
            case CustomBuild.BuildStage.IDLE:
                CreateCustomBuildUI();
                break;
            case CustomBuild.BuildStage.UNITY_EXPORT:
                GUI.Label(new Rect(5, 30, 590, 40), 
                          "building gradle project...\nPlease be patient as " +
                          "Unity might stop responding...\nThis process will " +
                          "launch external windows so that you can keep " +
                          "tracking the build progress");
                break;
            case CustomBuild.BuildStage.PROJECT_BUILD:
                GUI.Label(new Rect(5, 30, 590, 40), "Running gradle to " +
                          "generate APK...\nPlease be patient...");
                break;
            case CustomBuild.BuildStage.PROJECT_INSTALL:
                GUI.Label(new Rect(5, 30, 590, 40), "Installing APK...\n" +
                          "Please be patient...");
                break;
            case CustomBuild.BuildStage.PROJECT_RUN:
                GUI.Label(new Rect(5, 30, 590, 40), "Running APK...\nPlease " +
                          "be patient...");
                break;
            case CustomBuild.BuildStage.DONE:
                this.Close();
                break;
        }
    }

    void CreateCustomBuildUI()
    {
        //GRADLE
        float gradlePartHeight = 5;
        GUI.Label(new Rect(5, gradlePartHeight, 590, 40), 
                  "Select the gradle path:");
        
        gradlePartHeight += 20;
        CustomBuild.gradlePath = GUI.TextField(
            new Rect(5, gradlePartHeight, 590, 20), 
            CustomBuild.gradlePath);
        
        CustomBuild.gradlePath = 
            CustomBuildWindow.HandleCopyPaste(GUIUtility.keyboardControl) ?? 
            CustomBuild.gradlePath;
        
        gradlePartHeight += 20;
        CustomBuild.buildRelease = GUI.Toggle(
                new Rect(5, gradlePartHeight, 590, 20), 
                CustomBuild.buildRelease, 
                "Build a Release version? (Uncheck it if you want to build a " +
                "Debug Version)."
        );

        gradlePartHeight += 20;
        CustomBuild.debugMode = GUI.Toggle(
            new Rect(5, gradlePartHeight, 590, 20), 
            CustomBuild.debugMode,
            "Run gradle in debug mode? This will not end gradle terminal " +
            "automatically."
        );

        gradlePartHeight += 20;
        GUI.Label(new Rect(5, gradlePartHeight, 105, 20), "Gradle heap size:");
        CustomBuild.gradleMem = GUI.TextField(
            new Rect(105, gradlePartHeight, 60, 20), 
            CustomBuild.gradleMem
        );
        GUI.Label(new Rect(165, gradlePartHeight, 70, 20), "MB");

        gradlePartHeight += 25;
        GUI.Label(new Rect(5, gradlePartHeight, 150, 20), "Dex heap size:");
        CustomBuild.dexMem = GUI.TextField(
            new Rect(105, gradlePartHeight, 60, 20), 
            CustomBuild.dexMem
        );
        GUI.Label(new Rect(165, gradlePartHeight, 590, 20), 
                  "MB  (Gradle heap size has to be grater than Dex heap size)");

        //ADB
        float adbPartHeight = gradlePartHeight + 50;
        GUI.Label(new Rect(5, adbPartHeight, 590, 40), "Select the adb path:");

        adbPartHeight += 20;
        CustomBuild.adbPath = GUI.TextField(new Rect(5, adbPartHeight, 590, 20),
                                            CustomBuild.adbPath);
        CustomBuild.adbPath = 
            CustomBuildWindow.HandleCopyPaste(GUIUtility.keyboardControl) ?? 
            CustomBuild.adbPath;
        
        adbPartHeight += 20;
        CustomBuild.runAdbInstall = GUI.Toggle(
            new Rect(5, adbPartHeight, 590, 20), 
            CustomBuild.runAdbInstall, 
            "Install build when done?"
        );

        float adbRunPartHeight = adbPartHeight + 20;
        GUI.Label(new Rect(5, adbRunPartHeight, 590, 40), 
                  "Path to the main activity name (.UnityPlayerActivity " +
                  "by default)"
                 );
        
        adbRunPartHeight += 20;
        CustomBuild.mainActivityPath = GUI.TextField(
            new Rect(5, adbRunPartHeight, 590, 20), 
            CustomBuild.mainActivityPath
        );
        CustomBuild.mainActivityPath = 
            CustomBuildWindow.HandleCopyPaste(GUIUtility.keyboardControl) ?? 
            CustomBuild.mainActivityPath;
        
        adbRunPartHeight += 20;
        CustomBuild.runAdbRun = GUI.Toggle(
            new Rect(5, adbRunPartHeight, 590, 20), 
            CustomBuild.runAdbRun, 
            "Run build when done?");

        // SCENES
        float scenesPartHeight = adbRunPartHeight + 40;
        GUI.Label(new Rect(5, scenesPartHeight, 590, 40), 
                  "Select what scenes you want to export:\n(Only scenes that " +
                  "are in build settings are true by default)");
        
        int scenesLength = EditorBuildSettings.scenes.Length;
        float scrollViewLength = scenesLength * 25f;
        scenesPartHeight += 30;
        scrollViewVector = GUI.BeginScrollView(
            new Rect(5, scenesPartHeight, 590, 215), 
            scrollViewVector, 
            new Rect(0, 0, 500, scrollViewLength)
        );

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            ExportScenes.buildScenesEnabled[i] = GUI.Toggle(
                new Rect(10, 10 + i * 20, 500, 20), 
                ExportScenes.buildScenesEnabled[i], 
                EditorBuildSettings.scenes[i].path
            );
        }

        ExportScenes.UpdatedBuildScenes(ExportScenes.buildScenesEnabled);
        GUI.EndScrollView();

        // BUTTONS
        if (GUI.Button(new Rect(5, 470, 100, 20), "Player Settings"))
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        }

        if(GUI.Button(new Rect(115, 470, 120, 20), "Add Open Scenes"))
        {
            ExportScenes.AddAllOpenScenesToBuildSettings();
            ExportScenes.buildScenesEnabled = ExportScenes.GetScenesEnabled();
        }

        if (GUI.Button(new Rect(460, 470, 60, 20), "Cancel"))
        {
            this.Close();
        }

        if (CustomBuild.gradlePath != "" && 
            GUI.Button(new Rect(530, 470, 60, 20), "Confirm")
           )
        {
            CustomBuild.SetCustomBuildPrefs();
            CustomBuild.continueProcessEvent.Invoke();
            this.Close();
        }
    }

    private static string HandleCopyPaste(int controlID)
    {
        if (controlID == GUIUtility.keyboardControl)
        {
            if (Event.current.type == EventType.KeyUp && 
                (Event.current.modifiers == EventModifiers.Control ||
                 Event.current.modifiers == EventModifiers.Command
                )
               )
            {
                if (Event.current.keyCode == KeyCode.C)
                {
                    Event.current.Use();
                    TextEditor editor = (TextEditor)
                        GUIUtility.GetStateObject(typeof(TextEditor),
                                                  GUIUtility.keyboardControl
                                                 );
                    editor.Copy();
                }

                else if (Event.current.keyCode == KeyCode.V)
                {
                    Event.current.Use();
                    TextEditor editor = (TextEditor)
                        GUIUtility.GetStateObject(typeof(TextEditor), 
                                                  GUIUtility.keyboardControl
                                                 );
                    editor.Paste();
                    return editor.text;
                }
            }
        }
        
        return null;
    }
}