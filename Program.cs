using FastConsole;
using RogueProject.Utils;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Views;

namespace RogueProject;

public static class Program
{
    public static void Main()
    {
        var world = World.Instance;
        var worldController = new WorldController(world);
        var worldRenderer = new WorldRenderer(world);

        var playerController = new PlayerController(world, world.Entities[0]);

        var uiRenderer = new UiRenderer(world.Entities[0] as Player);

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
