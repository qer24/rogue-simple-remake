using RogueProject.Controllers;
using RogueProject.Models.Entities;
using RogueProject.Views;
using RogueProject.Utils;

namespace RogueProject.Models;

public class World
{
    public Room[] Rooms;
    public WorldCell[,] WorldGrid;
    public List<Entity> Entities;
    public List<Item> Items;

    public Player Player => (Player)Entities[0];

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
        return WorldGrid[Player.Position.x, Player.Position.y];
    }

    /// <summary>
    /// Get entity on cell based on position.
    /// </summary>
    public Entity GetEntityOnCell(Vector2Int position)
    {
        return Entities.FirstOrDefault(e => e.Position == position);
    }

    /// <summary>
    /// Get entity on cell based on position.
    /// </summary>
    public Entity GetEntityOnCell(int x, int y)
    {
        return Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);
    }

    /// <summary>
    /// Get item on cell based on position.
    /// </summary>
    public Item GetItemOnCell(Vector2Int position)
    {
        return Items.FirstOrDefault(i => i.Position == position);
    }

    /// <summary>
    /// Get item on cell based on position.
    /// </summary>
    public Item GetItemOnCell(int x, int y)
    {
        return Items.FirstOrDefault(i => i.Position.x == x && i.Position.y == y);
    }

    public IRenderable GetRenderableOnCell(Vector2Int position)
    {
        return (IRenderable)GetEntityOnCell(position) ?? GetItemOnCell(position);
    }

    public IRenderable GetRenderableOnCell(int x, int y)
    {
        return (IRenderable)GetEntityOnCell(x, y) ?? GetItemOnCell(x, y);
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

        // check if out of bounds
        if (x < 0 || x >= Constants.WORLD_SIZE.x || y < 0 || y >= Constants.WORLD_SIZE.y)
        {
            return true;
        }

        var tileType = WorldGrid[x, y].TileType;

        return tileType is TileType.WallTop
            or TileType.WallBottom
            or TileType.WallVertical
            or TileType.Empty;
    }

    /// <summary>
    /// Returns true if the given position is an enemy.
    /// </summary>
    /// <param name="pos"></param>
    public bool EnemyCheck(Vector2Int pos)
    {
        var x = pos.x;
        var y = pos.y;

        var entity = Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);

        return entity is Enemy;
    }

    /// <summary>
    /// Attack between two entities.
    /// </summary>
    public void Attack(Entity attacker, Entity target)
    {
        var attackDamage = CalculateDamage(attacker.Strength, target.Armor);
        var defenceDamage = CalculateDamage(target.Strength, attacker.Armor);

        target.ChangeHealth(-attackDamage);
        attacker.ChangeHealth(-defenceDamage);

        void KillEntity(Entity entity, int damage)
        {
            Entities.Remove(entity);
            UiMessage.Instance.ShowMessage($"      {entity.Name} -{damage} HP, {entity.Name} has been defeated", 5);

            if (entity is Enemy enemy)
            {
                Player.AddExperience(enemy.Experience);
                Player.Gold += enemy.Gold;
            }
        }

        if (target.Health <= 0)
        {
            KillEntity(target, attackDamage);
        }
        else if (attacker.Health <= 0)
        {
            KillEntity(attacker, defenceDamage);
        }
        else
        {
            UiMessage.Instance.ShowMessage($"      {target.Name} -{attackDamage} HP, {attacker.Name} -{defenceDamage} HP", 5);
        }
    }

    private static int CalculateDamage(float strength, float armor)
    {
        // Calculate armor effectiveness
        float damageReduction = 1 - 1 / (1 + armor * Constants.ARMOR_SCALING_FACTOR);

        // Apply armor penetration based on strength difference
        if (strength > armor)
        {
            float penetration = (strength - armor) * Constants.PENETRATION_FACTOR;
            damageReduction = Math.Max(0, damageReduction - penetration * Constants.ARMOR_SCALING_FACTOR);
        }

        // Cap the damage reduction
        damageReduction = Math.Min(damageReduction, Constants.MAX_DAMAGE_REDUCTION);

        // Calculate final damage and round to nearest integer
        float finalDamage = strength * (1 - damageReduction);

        // Round to nearest integer and ensure damage is never negative
        return Math.Max(0, (int)Math.Round(finalDamage));
    }
}
