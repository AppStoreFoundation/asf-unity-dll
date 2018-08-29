public class CustomBuildAndroidErrorWindow : CustomBuildErrorWindow
{
    protected string[] customErrorTitles = {
        "Export Unity Project: ",
        "(GRADLE) Build Exported Project: ",
        "(ADB) Install .apk to device: ",
        "(ADB) Run .apk in the device: "
    };

    protected override void ErrorGUI(string [] errorTitles)
    {
        base.ErrorGUI(customErrorTitles);
    }
}