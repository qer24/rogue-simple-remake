using FastConsole;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Views;

namespace RogueProject;

public static class Program
{
    public static void Main()
    {
        Console.WindowHeight = Constants.WORLD_SIZE.y + 2;
        Console.WindowWidth = Constants.WORLD_SIZE.x + 1;

        FConsole.Initialize("Rogue Project", Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);

        var world = new World();
        var worldController = new WorldController(world);
        var worldRenderer = new WorldRenderer(world);

        var player = world.Entities[0] as Player;
        var playerController = new PlayerController(world, player);
        var uiRenderer = new UiRenderer(player);

        void Update(bool firstUpdate = false)
        {
            if (!firstUpdate)
                playerController.Update();
            worldController.Update();
            worldRenderer.Render();
            uiRenderer.Render();

            FConsole.DrawBuffer();
        }

        // force first update before players makes any input
        Update(true);

        while (true)
        {
            Update();
        }
    }
}
