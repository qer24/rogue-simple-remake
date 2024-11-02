using RogueProject.Utils;

namespace RogueProject.Models;

public struct WorldCell
{
    public Vector2Int Position;
    public TileType TileType;
}

public enum TileType
{
    Empty,
    WallTop,
    WallBottom,
    WallVertical,
    Door,
    Corridor,
    Floor
}
