using RogueProject.Utils;

namespace RogueProject.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class UiMessage : Singleton<UiMessage>
{
    public string Message = "";
    public int RemainingDuration = 0;
    public int MaxDuration= 0;

    public void ShowMessage(string message, int duration)
    {
        Message = message;
        RemainingDuration = duration;
        MaxDuration = duration;
    }
}
