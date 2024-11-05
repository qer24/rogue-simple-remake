using FastConsole;
using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Views;

public class WorldRenderer(World world) : Renderer
{
    /// <summary>
    /// Generate a grid of characters based of the world and render it to the screen.
    /// </summary>
    public override void Render()
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var entity = world.Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);

                if (entity != null)
                {
                    var character = entity.Character;
                    var color = entity.Color;
                    FConsole.SetChar(x, y, character, color, Constants.BACKGROUND_COLOR);
                }
                else
                {
                    var cell = world.GetCell(new Vector2Int(x, y));

                    if (!cell.DoRender())
                    {
                        FConsole.SetChar(x, y, ' ', Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);
                        continue;
                    }

                    var tileType = cell.TileType;
                    var tileChar = GetTileChar(tileType);
                    FConsole.SetChar(x, y, tileChar, Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);
                }
            }
        }
    }

    private static char GetTileChar(TileType tileType)
    {
        return tileType switch
        {
            TileType.Floor => '.',
            TileType.WallTop => '-',
            TileType.WallBottom => '-',
            TileType.WallVertical => '|',
            TileType.Door => '+',
            TileType.Corridor => 'o',
            TileType.Empty => ' ',
            TileType.Stairs => '%',
            _ => '?'
        };
    }
}
