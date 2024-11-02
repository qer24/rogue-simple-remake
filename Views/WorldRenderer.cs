using FastConsole;
using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Views;

public class WorldRenderer
{
    private const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    private const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;

    public WorldRenderer()
    {
        FConsole.Initialize("Rogue Project", FOREGROUND_COLOR, BACKGROUND_COLOR);
    }

    public void RenderWorld(World world)
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        var grid = new char[sizeY, sizeX];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var entity = world.Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);

                if (entity != null)
                {
                    grid[y, x] = '@';
                }
                else
                {
                    var tileType = world.GetCell(new Vector2Int(x, y)).TileType;
                    grid[y, x] = GetTileChar(tileType);
                }
            }
        }

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                FConsole.SetChar((short)x, (short)y, grid[y, x], FOREGROUND_COLOR, BACKGROUND_COLOR);
            }
        }

        FConsole.DrawBuffer();
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
            TileType.Corridor => '#',
            TileType.Empty => ' ',
            _ => '?'
        };
    }
}
