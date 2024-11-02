using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Controllers;

public class WorldController : Controller
{
    private readonly World _world;
    private Vector2Int _playerStartPos;

    private readonly HashSet<Vector2Int> _visibleCells = [];

    // rooms i, j are neighbours if _neighbourMatrix[i, j] is true
    private readonly bool[,] _neighbourMatrix = new bool[9, 9]
    {
        { false, true,  false, true,  false, false, false, false, false },
        { true,  false, true,  false, true,  false, false, false, false },
        { false, true,  false, false, false, true,  false, false, false },
        { true,  false, false, false, true,  false, true,  false, false },
        { false, true,  false, true,  false, true,  false, true,  false },
        { false, false, true,  false, true,  false, false, false, true  },
        { false, false, false, true,  false, false, false, true,  false },
        { false, false, false, false, true,  false, true,  false, true  },
        { false, false, false, false, false, true,  false, true,  false }
    };

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
                    TileType = TileType.Empty,
                    Visible = false,
                    Revealed = false
                };
            }
        }

        GenerateWorld();

        var player = new Entity("Player", _playerStartPos);
        _world.Entities.Add(player);
    }

    public override void Update()
    {
        PlayerLineOfSight();
        PlayerCorridorReveal();
        PlayerRoomReveal();
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

                if (Vector2Int.Distance(player.Position, cell.Position) < playerVision)
                {
                    _visibleCells.Add(cell.Position);
                    _world.WorldGrid[x, y].Visible = true;
                }
            }
        }
    }

    private void PlayerCorridorReveal()
    {
        // reveal corridors
        foreach (var cell in _world.GetPlayerSurroundedTiles())
        {
            var worldCell = _world.WorldGrid[cell.x, cell.y];
            if (worldCell.TileType == TileType.Corridor)
            {
                _world.WorldGrid[cell.x, cell.y].Revealed = true;
            }
        }
    }

    private void PlayerRoomReveal()
    {
        // reveal rooms
        foreach (var cell in _world.GetPlayerSurroundedTiles())
        {
            var worldCell = _world.WorldGrid[cell.x, cell.y];
            if (worldCell.TileType != TileType.Door) continue;

            if (_world.TryGetRoom(worldCell, out var room))
            {
                _world.RevealRoom(room);
            }
        }
    }

    /// <summary>
    /// Procedural generation of the world
    /// </summary>
    private void GenerateWorld()
    {
        var rooms = new Room[9];

        Vector2Int IndexToPosition(int i)
        {
            var x = i % 3 * 26 + 1;
            var y = i / 3 * 8;

            return new Vector2Int(x, y);
        }

        // generate rooms
        var rng = new Random();
        var goneCount = 0;
        var maxGoneRooms = rng.Next(1, Constants.MAX_GONE_ROOMS + 1);
        for (int i = 0; i < rooms.Length; i++)
        {
            // rooms[i].Position = IndexToPosition(i); // top left corner
            // rooms[i].Size = new Vector2Int(26, 8);

            var position = IndexToPosition(i);
            var width = rng.Next(5, 25);
            var height = rng.Next(4, 8);

            position.x += rng.Next(0, 26 - width);
            position.y += rng.Next(0, 8 - height);

            rooms[i] = new Room(position, new Vector2Int(width, height));

            if (goneCount < maxGoneRooms)
            {
                var isGone = rng.Next(0, 2) == 1;
                if (!isGone) continue;

                rooms[i].Gone = true;
                goneCount++;
            }
        }

        // remove gone rooms
        var remainingRooms = rooms.Where(r => !r.Gone).ToArray();

        // set wall and floor tiles
        SetRooms(remainingRooms);
        _world.Rooms = remainingRooms;

        var mst = GenerateSpanningTree(rooms, remainingRooms, rng);
        ConnectRooms(mst, rooms);

        SpawnPlayer(rng, remainingRooms);
    }

    private void SetRooms(Room[] rooms)
    {
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
    }

    private List<(int i, int j)> GenerateSpanningTree(Room[] rooms, Room[] remainingRooms, Random rng)
    {
        // create minimum spanning tree between rooms, using BFS and exluding gone rooms
        var mst = new List<(int i, int j)>();
        var visited = new bool[9];
        var remainingRoomIndices = remainingRooms.Select(r => Array.IndexOf(rooms, r)).ToList();

        // Helper function to calculate distance between rooms
        double GetDistance(Room r1, Room r2)
        {
            int dx = r1.Position.x - r2.Position.x;
            int dy = r1.Position.y - r2.Position.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        while (remainingRoomIndices.Any(i => !visited[i]))
        {
            var queue = new Queue<int>();

            // Find first unvisited room from remaining rooms
            var startRoomIndex = remainingRoomIndices.First(i => !visited[i]);
            queue.Enqueue(startRoomIndex);
            visited[startRoomIndex] = true;

            while (queue.Count > 0)
            {
                var i = queue.Dequeue();

                // Check neighbours
                for (int j = 0; j < 9; j++)
                {
                    if (i == j || visited[j] || rooms[i].Gone || rooms[j].Gone) continue;

                    if (_neighbourMatrix[i, j])
                    {
                        // Visit neighbour
                        mst.Add((i, j));
                        queue.Enqueue(j);
                        visited[j] = true;
                    }
                }
            }

            // If there are still unvisited rooms, connect the components
            var unvisitedRemaining = remainingRoomIndices.Where(i => !visited[i]).ToList();
            if (unvisitedRemaining.Any())
            {
                // Find closest visited room to connect with
                var visitedRooms = remainingRoomIndices.Where(i => visited[i]).ToList();
                var nextRoom = unvisitedRemaining[0];

                // Connect to the closest visited room
                var closestVisited = visitedRooms.MinBy(i => GetDistance(rooms[i], rooms[nextRoom]));

                // Add connection between components
                mst.Add((closestVisited, nextRoom));
                // Mark the next starting room as visited
                visited[nextRoom] = true;
            }
        }

        bool CanConnect((int i, int j) connection)
        {
            var reversed = (connection.i, connection.j);

            return !mst.Contains(connection) && !mst.Contains(reversed);
        }

        // indexes from the original rooms array
        var remainingIndexes = remainingRooms.Select(r => Array.IndexOf(rooms, r)).ToArray();

        // also connect RANDOM_CONNECTION_COUNT random rooms
        for (int i = 0; i < Constants.RANDOM_CONNECTION_COUNT; i++)
        {
            var randomIndex1 = rng.Next(0, remainingIndexes.Length);
            var randomIndex2 = rng.Next(0, remainingIndexes.Length);

            var connection = (remainingIndexes[randomIndex1], remainingIndexes[randomIndex2]);

            if (!CanConnect(connection)) // already connected
            {
                i--;
                continue;
            }
            mst.Add(connection);
        }

        Logger.Log("MST:");
        foreach (var edge in mst)
        {
            Logger.Log($"{edge.i} -> {edge.j}");
        }

        return mst;
    }

    private void ConnectRooms(List<(int i, int j)> mst, Room[] rooms)
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        var aStarGrid = new Grid2D<AStar2D.Node>(new Vector2Int(sizeX, sizeY));
        var aStar = new AStar2D(aStarGrid);

        foreach ((int i, int j) in mst)
        {
            var roomA = rooms[i];
            var roomB = rooms[j];

            var startPos = roomA.Position + roomA.Size / 2;
            var endPos = roomB.Position + roomB.Size / 2;

            var path = aStar.FindPath(startPos, endPos, (_, neighbour) =>
            {
                var pathCost = new AStar2D.PathCost
                {
                    cost = Vector2Int.Distance(neighbour.Position, endPos) //heuristic
                };

                var pos = neighbour.Position;
                // cost function
                // prioritize going through already made paths
                // walls are more expensive to go through
                pathCost.cost += _world.WorldGrid[pos.x, pos.y].TileType switch
                {
                    TileType.WallTop      => 1000,
                    TileType.WallBottom   => 1000,
                    TileType.WallVertical => 1000,
                    TileType.Floor        => 1,
                    TileType.Door         => 750,
                    TileType.Empty        => 10,
                    TileType.Corridor     => 5,
                    _                     => 1000
                };

                pathCost.traversable = true;

                return pathCost;
            });

            foreach (var pos in path)
            {
                _world.WorldGrid[pos.x, pos.y].TileType = _world.WorldGrid[pos.x, pos.y].TileType switch
                {
                    TileType.Empty                                                   => TileType.Corridor,
                    TileType.WallTop or TileType.WallBottom or TileType.WallVertical => TileType.Door,
                    _                                                                => _world.WorldGrid[pos.x, pos.y].TileType
                };

                if (_world.WorldGrid[pos.x, pos.y].TileType == TileType.Corridor)
                {
                    _world.WorldGrid[pos.x, pos.y].Visible = true;
                }
            }
        }
    }

    private void SpawnPlayer(Random rng, Room[] remainingRooms)
    {
        // set player pos to middle of random room
        var randomRoomIndex = rng.Next(0, remainingRooms.Length);
        var playerRoom = remainingRooms[randomRoomIndex];
        _playerStartPos = playerRoom.Position + playerRoom.Size / 2;

        _world.RevealRoom(playerRoom);
    }
}
