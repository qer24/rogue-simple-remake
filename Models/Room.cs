using RogueProject.Utils;

namespace RogueProject.Models;

public struct Room
{
    public Vector2Int Position;
    public Vector2Int Size;
    public bool Gone;

    public Room(Vector2Int position, Vector2Int size)
    {
        Position = position;
        Size = size;
    }

    /// <summary>
    /// Gets random position in the room exluding the walls
    /// </summary>
    /// <returns></returns>
    public Vector2Int RandomPosition()
    {
        var rng = new Random();
        var x = rng.Range(Position.x + 1, Position.x + Size.x - 1);
        var y = rng.Range(Position.y + 1, Position.y + Size.y - 1);

        return new Vector2Int(x, y);
    }

    private bool Equals(Room other)
    {
        return Position.Equals(other.Position) && Size.Equals(other.Size) && Gone == other.Gone;
    }

    public override bool Equals(object obj)
    {
        return obj is Room other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Size, Gone);
    }

    public static bool operator ==(Room left, Room right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Room left, Room right)
    {
        return !left.Equals(right);
    }
}
