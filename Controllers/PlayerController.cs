using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Controllers;

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

        var newCell = world.GetCell(newPosition);
        var tileType = newCell.TileType;

        if (tileType != TileType.WallTop && tileType != TileType.WallBottom && tileType != TileType.WallVertical && tileType != TileType.Empty)
        {
            player.Position = newPosition;
        }
    }
}
