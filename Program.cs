using ProjektFB.Controllers;
using ProjektFB.Models;
using ProjektFB.Utils;
using ProjektFB.Views;

namespace ProjektFB;

public static class Program
{
    public static void Main()
    {
        var world = World.Instance;
        var worldController = new WorldController(world);
        var playerController = new PlayerController(world, world.Entities[0]);

        while (true)
        {
            WorldRenderer.RenderWorld(world);
            playerController.Update();
            worldController.Update();
        }
    }
}
