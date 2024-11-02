using RogueProject.Controllers;
using RogueProject.Views;
using RogueProject.Utils;

namespace RogueProject.Models;

public class World : Singleton<World>
{
    public Room[] Rooms;
    public WorldCell[,] WorldGrid;
    public List<Entity> Entities;

    public WorldCell GetCell(Vector2Int position)
    {
        return WorldGrid[position.x, position.y];
    }

    public void RevealRoom(Room room)
    {
        var x = room.Position.x;
        var y = room.Position.y;
        var width = room.Size.x;
        var height = room.Size.y;

        for (var i = x; i < x + width; i++)
        {
            for (var j = y; j < y + height; j++)
            {
                WorldGrid[i, j].Revealed = true;
            }
        }
    }

    public Vector2Int[] GetPlayerSurroundedTiles()
    {
        var player = Entities[0];

        var surroundingTiles = new[]
        {
            new Vector2Int(player.Position.x, player.Position.y - 1),
            new Vector2Int(player.Position.x - 1, player.Position.y),
            new Vector2Int(player.Position.x + 1, player.Position.y),
            new Vector2Int(player.Position.x, player.Position.y + 1)
        };

        // remove any out of bounds tiles
        surroundingTiles = surroundingTiles.Where(t => t.x >= 0 && t.x < Constants.WORLD_SIZE.x && t.y >= 0 && t.y < Constants.WORLD_SIZE.y).ToArray();

        return surroundingTiles;
    }

    /// <summary>
    /// Pretty expensive, only call when player is near a door
    /// </summary>
    public bool TryGetRoom(WorldCell cell, out Room room)
    {
        if (cell.TileType != TileType.Door)
        {
            Logger.Log($"Trying to get a room from a non-door cell at {cell.Position}!");
        }

        if (cell.TileType is TileType.Corridor or TileType.Empty)
        {
            room = default;
            return false;
        }

        foreach (var r in Rooms)
        {
            if (r.Position.x <= cell.Position.x && r.Position.y <= cell.Position.y &&
                r.Position.x + r.Size.x >= cell.Position.x && r.Position.y + r.Size.y >= cell.Position.y)
            {
                room = r;
                return true;
            }
        }

        room = default;
        return false;
    }
}
