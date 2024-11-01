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

        var worldRenderer = new WorldRenderer();

        while (true)
        {
            worldRenderer.RenderWorld(world);
            playerController.Update();
            worldController.Update();
        }
    }
}
