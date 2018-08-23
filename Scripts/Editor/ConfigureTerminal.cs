using Aptoide.AppcoinsUnity;

public class ConfigureTerminal : IConfigureTerminal
{
    public void SetTerminalAndRun(BuildStage stage, TerminalSelected tSel, 
                                  string command, string path, string args)
    {
        Terminal terminal = null;

        if (tSel == TerminalSelected.CMD)
        {
            terminal = new CMD();
        }

        else if (tSel == TerminalSelected.BASH)
        {
            terminal = new Bash();
        }

        terminal.RunCommand(stage, command, args, path, false);
    }
}