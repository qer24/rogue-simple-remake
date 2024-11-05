using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueProject.Models;

public class WorldGenerator(World world)
{
    private Entity _player;
    private Vector2Int _playerStartPos;

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

    /// <summary>
    /// Procedural generation of the world
    /// </summary>
    public void GenerateWorld(bool regenerate)
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        world.WorldGrid = new WorldCell[sizeX, sizeY];
        if (world.Entities != null) _player = world.Entities[0];
        world.Entities = [];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                world.WorldGrid[x, y] = new WorldCell
                {
                    Position = new Vector2Int(x, y),
                    TileType = TileType.Empty,
                    Visible = false,
                    Revealed = false
                };
            }
        }

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
        world.Rooms = remainingRooms;

        var mst = GenerateSpanningTree(rooms, remainingRooms, rng);
        ConnectRooms(mst, rooms);

        SpawnPlayer(rng, remainingRooms, out var playerStartRoom, regenerate);
        PlaceStairs(rng, rooms, remainingRooms, playerStartRoom);
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
                        world.WorldGrid[x, y].TileType = TileType.WallTop;
                    }
                    else if (y == room.Position.y + room.Size.y - 1)
                    {
                        world.WorldGrid[x, y].TileType = TileType.WallBottom;
                    }
                    else if (x == room.Position.x || x == room.Position.x + room.Size.x - 1)
                    {
                        world.WorldGrid[x, y].TileType = TileType.WallVertical;
                    }
                    else
                    {
                        world.WorldGrid[x, y].TileType = TileType.Floor;
                    }
                }
            }
        }
    }

    private List<(int i, int j)> GenerateSpanningTree(Room[] rooms, Room[] remainingRooms, Random rng)
    {
        // create minimum spanning tree between rooms, using BFS and exluding gone rooms
        var mst = new List<(int i, int j)>();
        var remainingRoomIndices = remainingRooms.Select(r => Array.IndexOf(rooms, r)).ToList();

        // Function to calculate distance between rooms
        double GetDistance(Room r1, Room r2)
        {
            int dx = r1.Position.x - r2.Position.x;
            int dy = r1.Position.y - r2.Position.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Create list of all possible edges between remaining rooms
        var edges = new List<(int from, int to, double distance)>();
        for (int i = 0; i < remainingRoomIndices.Count; i++)
        {
            for (int j = i + 1; j < remainingRoomIndices.Count; j++)
            {
                int roomI = remainingRoomIndices[i];
                int roomJ = remainingRoomIndices[j];

                // Skip if rooms are gone
                if (rooms[roomI].Gone || rooms[roomJ].Gone) continue;

                // Calculate distance
                double distance = GetDistance(rooms[roomI], rooms[roomJ]);
                edges.Add((roomI, roomJ, distance));
            }
        }

        // Sort edges by distance
        edges = edges.OrderBy(e => e.distance).ToList();

        // Initialize DisjointSet
        var disjointSet = new DisjointSet(9);

        // Kruskal's algorithm
        foreach (var edge in edges)
        {
            // Skip if rooms are already connected
            if (disjointSet.Find(edge.from) == disjointSet.Find(edge.to)) continue;

            mst.Add((edge.from, edge.to));
            disjointSet.Union(edge.from, edge.to);

            _neighbourMatrix[edge.from, edge.to] = true;
            _neighbourMatrix[edge.to, edge.from] = true;
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
                pathCost.cost += world.WorldGrid[pos.x, pos.y].TileType switch
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
                world.WorldGrid[pos.x, pos.y].TileType = world.WorldGrid[pos.x, pos.y].TileType switch
                {
                    TileType.Empty                                                   => TileType.Corridor,
                    TileType.WallTop or TileType.WallBottom or TileType.WallVertical => TileType.Door,
                    _                                                                => world.WorldGrid[pos.x, pos.y].TileType
                };

                if (world.WorldGrid[pos.x, pos.y].TileType == TileType.Corridor)
                {
                    world.WorldGrid[pos.x, pos.y].Visible = true;
                }
            }
        }
    }

    private void SpawnPlayer(Random rng, Room[] remainingRooms, out Room playerRoom, bool regenerate)
    {
        // set player pos to middle of random room
        var randomRoomIndex = rng.Next(0, remainingRooms.Length);
        playerRoom = remainingRooms[randomRoomIndex];
        _playerStartPos = playerRoom.Position + playerRoom.Size / 2;

        world.RevealRoom(playerRoom);

        if (regenerate)
        {
            _player.Position = _playerStartPos;
        }
        else
        {
            _player = new Player(nameof(Player), _playerStartPos);
        }
        world.Entities.Add(_player);
    }

    private void PlaceStairs(Random rng, Room[] allRooms, Room[] remainingRooms, Room playerRoom)
    {
        // remove player spawn room
        var validRooms = remainingRooms.Where(r => r != playerRoom).ToArray();

        // remove neighbours of player room
        var playerRoomIndex = Array.IndexOf(allRooms, playerRoom);
        for (int i = 0; i < _neighbourMatrix.GetLength(0); i++)
        {
            if (validRooms.Length == 1) // to prevent removing all rooms
            {
                break;
            }

            if (_neighbourMatrix[playerRoomIndex, i])
            {
                validRooms = validRooms.Where(r => r != allRooms[i]).ToArray();
            }
        }

        // place stairs in random room
        var randomRoomIndex = rng.Range(0, validRooms.Length);
        var stairsRoom = validRooms.ElementAt(randomRoomIndex);

        // random pos in room
        var pos = stairsRoom.RandomPosition();

        world.WorldGrid[pos.x, pos.y].TileType = TileType.Stairs;
    }
}
