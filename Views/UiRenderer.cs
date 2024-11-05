using System.Text;
using FastConsole;
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

        var uiString = new StringBuilder();

        uiString.Append($" Level:{player.Level}{TAB}");
        uiString.Append($"Hp:{player.Health}/{player.MaxHealth}{TAB}");
        uiString.Append($"Str:{player.Strength}{TAB}");
        uiString.Append($"Gold:{player.Gold}{TAB}");
        uiString.Append($"Armor:{player.Armor}{TAB}");
        uiString.Append($"Exp:{player.Experience}/{player.ExperienceToNextLevel}{TAB}");

        for (int x = 0; x < uiString.Length; x++)
        {
            var character = uiString[x];
            FConsole.SetChar(x, y, character, Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);
        }
    }
}
