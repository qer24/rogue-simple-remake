using RogueProject.Utils;

namespace RogueProject.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class UiMessage : Singleton<UiMessage>
{
    public string Message = "";
    public int RemainingDuration = 0;
    public bool Priority = false;
    public int MaxDuration = 0;

    public void ShowMessage(string message, int duration, bool priority = false)
    {
        if (priority)
        {
            Priority = true;
        }
        else if (Priority)
        {
            return;
        }

        Message = message + new string(' ', 50);
        RemainingDuration = duration;
        MaxDuration = duration;
    }

    public void Reset()
    {
        Message = new string(' ', 100);
        RemainingDuration = 0;
        Priority = false;
        MaxDuration = 0;
    }
}
