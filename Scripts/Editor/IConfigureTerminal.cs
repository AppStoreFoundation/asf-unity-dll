public interface IConfigureTerminal
{
    void SetTerminalAndRun(BuildStage stage, TerminalSelected tSel, 
                           string command, string path, string args);
}