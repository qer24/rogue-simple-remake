using RogueProject.Utils;

namespace RogueProject.Models;

public struct WorldCell
{
    public Vector2Int Position;
    public TileType TileType;

    public bool Visible;
    public bool Revealed;

    /// <summary>
    /// The cell is rendered if it is revealed or visible if it's a floor.
    /// </summary>
    public bool DoRender()
    {
        // For floor to be visible, it must be revealed aswell as visible
        // For any other tile to be visible, it just must be revealed

        return TileType == TileType.Floor ? Revealed && Visible : Revealed;
    }
}

public enum TileType
{
    Empty,
    WallTop,
    WallBottom,
    WallVertical,
    Door,
    Corridor,
    Floor,
    Stairs
}
