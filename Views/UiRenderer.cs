using System.Text;
using FastConsole;
using RogueProject.Models;
using RogueProject.Models.Entities;

namespace RogueProject.Views;

public class UiRenderer(Player player) : Renderer
{
    private const string TAB = "       ";

    /// <summary>
    /// Generate the UI string and render it to the screen.
    /// </summary>
    public override void Render()
    {
        var y = Constants.WORLD_SIZE.y;

        var playerUi = new StringBuilder();

        playerUi.Append($" Level:{player.Level}{TAB}");
        playerUi.Append($"Hp:{player.Health}/{player.MaxHealth}{TAB}");
        playerUi.Append($"Str:{player.Strength}{TAB}");
        playerUi.Append($"Gold:{player.Gold}{TAB}");
        playerUi.Append($"Armor:{player.Armor}{TAB}");
        playerUi.Append($"Exp:{player.Experience}/{player.ExperienceToNextLevel}{TAB}");

        for (int x = 0; x < playerUi.Length; x++)
        {
            var character = playerUi[x];
            FConsole.SetChar(x, y, character, Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);
        }

        y++;

        var uiMessage = UiMessage.Instance;

        var message = uiMessage.Message;
        for (int x = 0; x < message.Length; x++)
        {
            var character = message[x];
            var xPosition = x + Constants.WORLD_SIZE.x / 4;
            FConsole.SetChar(xPosition, y, character, Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);
        }

        if (uiMessage.RemainingDuration <= 0)
        {
            uiMessage.Reset();
        }
        else
        {
            uiMessage.RemainingDuration--;
        }
    }
}
