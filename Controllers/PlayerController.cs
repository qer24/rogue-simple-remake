using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueProject.Controllers;

public class PlayerController(World world, Player player) : Controller
{
    private Vector2Int _movementDirection;

    /// <summary>
    /// Escape - Exit the game.
    /// R - Reveal the map.
    /// G - Generate a new world.
    /// Arrow keys - Move the player.
    /// </summary>
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
                world.GenerateWorld(true);
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
        Move(_movementDirection);
    }

    public void Move(Vector2Int direction)
    {
        var newPosition = player.Position + direction;

        if (!world.CollisionCheck(newPosition))
        {
            player.Position = newPosition;
        }
    }
}
