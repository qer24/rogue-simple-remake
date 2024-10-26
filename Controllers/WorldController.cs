using ProjektFB.Models;
using ProjektFB.Utils;

namespace ProjektFB.Controllers;

public class WorldController : Controller
{
    private readonly World _world;

    public WorldController(World world)
    {
        _world = world;

        Init();
    }

    private void Init()
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        _world.WorldGrid = new WorldCell[sizeX, sizeY];
        _world.Entities = [];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                _world.WorldGrid[x, y] = new WorldCell
                {
                    Position = new Vector2Int(x, y)
                };
            }
        }

        var player = new Entity("Player", new Vector2Int(sizeX / 2, sizeY / 2));
        _world.Entities.Add(player);
    }
}
