using RogueProject.Utils;

namespace RogueProject.Models;

public struct Room
{
    public Vector2Int Position;
    public Vector2Int Size;

    public Room(Vector2Int position, Vector2Int size)
    {
        Position = position;
        Size = size;
    }
}
