using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueProject.Controllers;

public class PlayerController(World world, Player player) : Controller
{
    private Vector2Int _movementDirection;

    private void GetInput()
    {
        _movementDirection = new Vector2Int(0, 0);
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.Escape:
                Environment.Exit(0);
                return;
            case ConsoleKey.R:
            {
                // reveal map
                foreach (var cell in world.WorldGrid)
                {
                    var x = cell.Position.x;
                    var y = cell.Position.y;

                    world.WorldGrid[x, y].Revealed = true;
                }
                return;
            }
            case ConsoleKey.G:
                var worldController = WorldController.Instance;
                worldController.GenerateWorld(true);
                return;
            default:
                _movementDirection = key switch
                {
                    ConsoleKey.UpArrow    => new Vector2Int(0, -1),
                    ConsoleKey.LeftArrow  => new Vector2Int(-1, 0),
                    ConsoleKey.DownArrow  => new Vector2Int(0, 1),
                    ConsoleKey.RightArrow => new Vector2Int(1, 0),
                    _                     => new Vector2Int(0, 0)
                };

                break;
        }
    }

    public override void Update()
    {
        GetInput();

        var newPosition = player.Position + _movementDirection;

        if (!world.WallCheck(newPosition))
        {
            player.Position = newPosition;
        }
    }
}
