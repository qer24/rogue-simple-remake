using FastConsole;
using RogueProject.Utils;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Views;

namespace RogueProject;

public static class Program
{
    public static void Main()
    {
        var world = World.Instance;
        var worldController = new WorldController(world);
        var playerController = new PlayerController(world, world.Entities[0]);

        var worldRenderer = new WorldRenderer(world);

        void Update(bool firstUpdate = false)
        {
            if (!firstUpdate)
                playerController.Update();
            worldController.Update();
            worldRenderer.RenderWorld();
            // uiRenderer.RenderUI();

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
