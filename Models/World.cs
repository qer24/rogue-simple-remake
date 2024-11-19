﻿using RogueProject.Controllers;
using RogueProject.Views;
using RogueProject.Utils;

namespace RogueProject.Models;

public class World
{
    public Room[] Rooms;
    public WorldCell[,] WorldGrid;
    public List<Entity> Entities;
    public List<Item> Items;

    /// <summary>
    /// Procedurally generates the world, if regenerate is true, the player reference is kept.
    /// </summary>
    public void GenerateWorld(bool regenerate)
    {
        var worldGenerator = new WorldGenerator(this);
        worldGenerator.GenerateWorld(regenerate);
    }

    /// <summary>
    /// Get cell based on position.
    /// </summary>
    /// <param name="position"></param>
    public WorldCell GetCell(Vector2Int position)
    {
        return WorldGrid[position.x, position.y];
    }

    /// <summary>
    /// Get the cell player is currently on.
    /// </summary>
    public WorldCell GetPlayercell()
    {
        return WorldGrid[Entities[0].Position.x, Entities[0].Position.y];
    }

    public Entity GetEntityOnCell(Vector2Int position)
    {
        return Entities.FirstOrDefault(e => e.Position == position);
    }

    public Item GetItemOnCell(Vector2Int position)
    {
        return Items.FirstOrDefault(i => i.Position == position);
    }

    /// <summary>
    /// Sets each cell in the room as revealed.
    /// </summary>
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

    /// <summary>
    /// Get the tiles in 4 cardinal directions around the player.
    /// </summary>
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
    /// Gets the room that contains the given cell.
    /// Pretty expensive, only call when player is near a door.
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

    /// <summary>
    /// Cast a line between two points,
    /// returns true if any wall is hit between them.
    /// </summary>
    /// <returns></returns>
    public bool Linecast(Vector2Int point1, Vector2Int point2)
    {
        // get all cells between the two points
        // use brasenham's line algorithm
        // if any cell is a wall, return true

        var x0 = point1.x;
        var y0 = point1.y;

        var x1 = point2.x;
        var y1 = point2.y;

        var dx = Math.Abs(x1 - x0);
        var dy = Math.Abs(y1 - y0);

        var sx = x0 < x1 ? 1 : -1;
        var sy = y0 < y1 ? 1 : -1;

        var err = dx - dy;

        while (true)
        {
            if (WorldGrid[x0, y0].TileType is TileType.WallTop or TileType.WallBottom or TileType.WallVertical)
            {
                return true;
            }

            if (x0 == x1 && y0 == y1)
            {
                break;
            }

            var e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the given position is a wall or empty.
    /// </summary>
    public bool CollisionCheck(Vector2Int pos)
    {
        var x = pos.x;
        var y = pos.y;

        var tileType = WorldGrid[x, y].TileType;

        return tileType is TileType.WallTop
            or TileType.WallBottom
            or TileType.WallVertical
            or TileType.Empty;
    }
}
