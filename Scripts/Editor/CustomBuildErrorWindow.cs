using UnityEditor;
using UnityEngine;

using System;

public class CustomBuildErrorWindow : EditorWindow
{
    protected static CustomBuildErrorWindow instance;
    protected CustomBuildErrorWindow innerInstance;
    protected BuildStage[] allStages = {BuildStage.UNITY_EXPORT,
        BuildStage.PROJECT_BUILD, BuildStage.PROJECT_INSTALL,
        BuildStage.PROJECT_RUN};

    protected BuildStage failStage;
    protected Exception error;

    public Vector2 scrollViewVector = Vector2.zero;

    protected internal BuildStage stage;

    private static string[] _errorsTitles = {
        "Export Unity Project: ",
        "(Gradle) Build Exported Project: ",
        "(ADB) Install .apk to device: ",
        "(ADB) Run .apk in the device: "
    };

    //Create the custom Editor Window
    public static void CreateCustomBuildErrorWindow(BuildStage fStage, 
                                                    Exception e)
    {
        CustomBuildErrorWindow.instance = (CustomBuildErrorWindow)
            EditorWindow.GetWindowWithRect(
                typeof(CustomBuildErrorWindow),
                new Rect(0, 0, 600, 500),
                true,
                "Custom Build Errors"
            );

        instance.failStage = fStage;
        instance.error = e;
        instance.minSize = new Vector2(600, 500);
        instance.autoRepaintOnSceneChange = true;
        instance.Show();
    }

    public void SetFailedBuildStage(BuildStage fStage)
    {
        failStage = fStage;
    }

    public void SetException(Exception e)
    {
        error = e;
    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    void OnGUI()
    {
        ErrorGUI();
    }

    protected virtual void ErrorGUI()
    {
        Texture2D success;
        Texture2D fail;

        int height = 10;
        int failStageIndex = ArrayUtility.IndexOf<BuildStage>(allStages,
                                                              failStage);

        int i = 0;
        while (i < allStages.Length)
        {
            GUI.Label(new Rect(5, height, 590, 20), _errorsTitles[i]);

            if (i < failStageIndex)
            {
                success = (Texture2D)Resources.Load(
                    "/Assets/AppcoinsUnity/icons/false.png", 
                    typeof(Texture2D)
                );

                GUI.DrawTexture(new Rect(5, height, 40, 40), success);
            }

            else
            {
                fail = (Texture2D)Resources.Load(
                    "/Assets/AppcoinsUnity/icons/false.png", 
                    typeof(Texture2D)
                );

                GUI.DrawTexture(new Rect(5, height, 40, 40), fail);
            }

            height += 10;
        }

        GUI.Label(new Rect(10, height, 590, height + 200), error.Message);

        if (GUI.Button(new Rect(530, 470, 60, 20), "Got it"))
        {
            this.Close();
        }
    }
}