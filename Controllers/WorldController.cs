using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueProject.Controllers;

public class WorldController : Controller
{
    private readonly World _world;
    private readonly HashSet<Vector2Int> _visibleCells = [];

    public WorldController(World world)
    {
        _world = world;

        Init();
    }

    private void Init()
    {
        _world.GenerateWorld(false);
    }

    public override void Update()
    {
        PlayerLineOfSight();
        PlayerReveal();
        PlayerStairs();
    }

    private void PlayerLineOfSight()
    {
        var player = _world.Entities[0];

        // hide all visible cells
        foreach (var cell in _visibleCells)
        {
            var x = cell.x;
            var y = cell.y;

            _world.WorldGrid[x, y].Visible = false;
        }

        // add cells within player's vision to visible cells
        _visibleCells.Clear();

        const int playerVision = Constants.FLOOR_REVEAL_DISTANCE;

        for (int x = player.Position.x - playerVision; x < player.Position.x + playerVision; x++)
        {
            for (int y = player.Position.y - playerVision; y < player.Position.y + playerVision; y++)
            {
                if (x < 0 || x >= Constants.WORLD_SIZE.x || y < 0 || y >= Constants.WORLD_SIZE.y) continue;

                var cell = _world.WorldGrid[x, y];
                if (cell.TileType != TileType.Floor) continue;

                if (!FastDistanceCheck(player.Position, cell.Position, playerVision)) continue;
                if (_world.Linecast(player.Position, cell.Position)) continue;

                _visibleCells.Add(cell.Position);
                _world.WorldGrid[x, y].Visible = true;
            }
        }
    }

    private static bool FastDistanceCheck(Vector2Int a, Vector2Int b, int distance)
    {
        var dx = a.x - b.x;
        var dy = a.y - b.y;

        return dx * dx + dy * dy < distance * distance;
    }

    private void PlayerReveal()
    {
        // reveal surrounding tiles
        foreach (var cell in _world.GetPlayerSurroundedTiles())
        {
            var worldCell = _world.WorldGrid[cell.x, cell.y];
            if (worldCell.TileType == TileType.Corridor) // reveal corridors
            {
                _world.WorldGrid[cell.x, cell.y].Revealed = true;
            }
            else if (worldCell.TileType == TileType.Door) // reveal rooms
            {
                if (_world.TryGetRoom(worldCell, out var room))
                {
                    _world.RevealRoom(room);
                }
            }
        }
    }

    private void PlayerStairs()
    {
        var playerCell = _world.GetPlayercell();

        if (playerCell.TileType != TileType.Stairs) return;

        _world.GenerateWorld(true);
        Update();
    }
}
