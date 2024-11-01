using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Controllers;

public class WorldController : Controller
{
    private readonly World _world;
    private Vector2Int _playerStartPos;

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
                    Position = new Vector2Int(x, y),
                    TileType = TileType.Empty
                };
            }
        }

        GenerateWorld();

        var player = new Entity("Player", _playerStartPos);
        _world.Entities.Add(player);
    }

    private void GenerateWorld()
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;
        var rooms = new Room[9];

        Vector2Int IndexToPosition(int i)
        {
            var x = (i % 3) * 26 + 1;
            var y = (i / 3) * 8;

            return new Vector2Int(x, y);
        }

        var rng = new Random();
        for (int i = 0; i < rooms.Length; i++)
        {
            // rooms[i].Position = IndexToPosition(i); // top left corner
            // rooms[i].Size = new Vector2Int(26, 8);

            var position = IndexToPosition(i);
            var width = rng.Next(4, 26);
            var height = rng.Next(4, 8);

            position.x += rng.Next(0, 26 - width);
            position.y += rng.Next(0, 8 - height);

            rooms[i] = new Room(position, new Vector2Int(width, height));
        }

        // set tile types
        foreach (var room in rooms)
        {
            for (int x = room.Position.x; x < room.Position.x + room.Size.x; x++)
            {
                for (int y = room.Position.y; y < room.Position.y + room.Size.y; y++)
                {
                    if (y == room.Position.y)
                    {
                        _world.WorldGrid[x, y].TileType = TileType.WallTop;
                    }
                    else if (y == room.Position.y + room.Size.y - 1)
                    {
                        _world.WorldGrid[x, y].TileType = TileType.WallBottom;
                    }
                    else if (x == room.Position.x || x == room.Position.x + room.Size.x - 1)
                    {
                        _world.WorldGrid[x, y].TileType = TileType.WallVertical;
                    }
                    else
                    {
                        _world.WorldGrid[x, y].TileType = TileType.Floor;
                    }
                }
            }
        }

        // set player pos to middle of room 0
        _playerStartPos = rooms[0].Position + rooms[0].Size / 2;
    }
}
