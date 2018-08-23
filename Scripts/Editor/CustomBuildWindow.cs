using UnityEditor;
using UnityEngine;

// Draw the window for the user select what scenes he wants to export
// and configure player settings.

public abstract class CustomBuildWindow : EditorWindow
{
    protected static CustomBuildWindow instance;
    public Vector2 scrollViewVector = Vector2.zero;

    protected internal SelectScenes selector;
    protected internal bool[] buildScenesEnabled = null;

    //Create the custom Editor Window
    public static void CreateCustomBuildWindow(CustomBuildWindow w, 
                                               SelectScenes sel)
    {
        CustomBuildWindow.instance = (CustomBuildWindow)
            EditorWindow.GetWindowWithRect(
                w.GetType(),
                new Rect(0, 0, 600, 500),
                true,
                "Custom Build Settings"
            );

        instance.PassScenesSelector(sel);
        instance.buildScenesEnabled = 
            instance.selector.GetBuildSettingsScenesEnabled();

        instance.minSize = new Vector2(600, 500);
        instance.autoRepaintOnSceneChange = true;
        instance.Show();
    }

    private void PassScenesSelector(SelectScenes sel)
    {
        selector = sel;
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
                IdleGUI();
                break;
            case CustomBuild.BuildStage.UNITY_EXPORT:
                UnityExportGUI();
                break;
            case CustomBuild.BuildStage.PROJECT_BUILD:
                ProjectBuildGUI();
                break;
            case CustomBuild.BuildStage.PROJECT_INSTALL:
                ProjectInstallGUI();
                break;
            case CustomBuild.BuildStage.PROJECT_RUN:
                ProjectRunGUI();
                break;
            case CustomBuild.BuildStage.DONE:
                Close();
                break;
        }
    }

    protected abstract void IdleGUI();

    protected abstract void UnityExportGUI();

    protected abstract void ProjectBuildGUI();

    protected abstract void ProjectInstallGUI();

    protected abstract void ProjectRunGUI();

    protected string HandleCopyPaste(int controlID)
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