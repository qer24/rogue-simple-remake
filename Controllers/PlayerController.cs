using ProjektFB.Models;
using ProjektFB.Utils;

namespace ProjektFB.Controllers;

public class PlayerController(World world, Entity player) : Controller
{
    private static Vector2Int GetInput()
    {
        var key = Console.ReadKey().Key;

        return key switch
        {
            ConsoleKey.UpArrow    => new Vector2Int(0, -1),
            ConsoleKey.LeftArrow  => new Vector2Int(-1, 0),
            ConsoleKey.DownArrow  => new Vector2Int(0, 1),
            ConsoleKey.RightArrow => new Vector2Int(1, 0),
            _                     => new Vector2Int(0, 0)
        };
    }

    public override void Update()
    {
        var input = GetInput();
        var newPosition = player.Position + input;

        if (newPosition.x < 1 || newPosition.x >= Constants.WORLD_SIZE.x - 1 ||
            newPosition.y < 1 || newPosition.y >= Constants.WORLD_SIZE.y - 1)
        {
            return;
        }

        player.Position = newPosition;
    }
}
