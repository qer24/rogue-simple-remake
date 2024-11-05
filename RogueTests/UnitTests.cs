using RogueProject;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueTests;

public class Tests
{
    [SetUp]
    public void Setup() { }

    [Test]
    public void CollisionTest()
    {
        var world = new World();
        world.GenerateWorld(false);

        var player = world.Entities[0];
        var playerController = new PlayerController(world, player as Player);

        // move towards the wall
        for (int i = 0; i < 20; i++)
        {
            playerController.Move(new Vector2Int(1, 0));
        }

        // test if we have a wall on the right
        var isWall = world.CollisionCheck(player.Position + new Vector2Int(1, 0));
        Assert.That(isWall);
    }

    [Test]
    public void MinimumSpanningTreeTest()
    {
        var world = new World();
        var worldGenerator = new WorldGenerator(world);

        const int checkIterations = 100;

        for (int i = 0; i < checkIterations; i++)
        {
            // generate new world
            worldGenerator.GenerateWorld(false);
            var rooms = world.Rooms;
            var roomCount = rooms.Length;

            // use a breadth-first search to check if all rooms are reachable
            var visited = new HashSet<int>();
            var queue = new Queue<int>();

            // start from the first room
            visited.Add(0);
            queue.Enqueue(0);

            while (queue.Count > 0)
            {
                var currentRoom = queue.Dequeue();

                // check all connected rooms
                for (int j = 0; j < roomCount; j++)
                {
                    if (worldGenerator.NeighbourMatrix[currentRoom, j] && visited.Add(j))
                    {
                        queue.Enqueue(j);
                    }
                }
            }

            // verify that all rooms were visited
            Assert.That(visited, Has.Count.EqualTo(roomCount), $"Not all rooms are reachable in iteration {i}");
        }
    }

    [Test]
    public void LinecastTest()
    {
        var world = new World();
        world.GenerateWorld(false);

        // linecast from top left to bottom right
        var topLeft = new Vector2Int(0, 0);
        var bottomRight = new Vector2Int(Constants.WORLD_SIZE.x - 1, Constants.WORLD_SIZE.y - 1);
        var linecast = world.Linecast(topLeft, bottomRight);

        // that line should hit a wall
        Assert.That(linecast);
    }

    [Test]
    public void RoomGenerationTest()
    {
        var world = new World();
        var worldGenerator = new WorldGenerator(world);
        worldGenerator.GenerateWorld(false);

        var rooms = world.Rooms;

        // we should have between 1 and 4 rooms
        Assert.That(rooms, Has.Length.InRange(1, 9));
    }

    [Test]
    public void WorldEntityGenerationTest()
    {
        var world = new World();
        world.GenerateWorld(false);

        var entities = world.Entities;

        // we should have at least 1 entity
        Assert.That(entities, Is.Not.Empty);
    }

    [Test]
    public void WorldGridTest()
    {
        var world = new World();
        world.GenerateWorld(false);

        var worldGrid = world.WorldGrid;

        // we should have a grid of cells
        Assert.That(worldGrid, Is.Not.Null);
    }

    [Test]
    public void PlayerTest()
    {
        var world = new World();
        world.GenerateWorld(false);

        // entity 0 should be the player
        var player = world.Entities[0] as Player;

        Assert.That(player, Is.Not.Null);
    }
}
