using FastConsole;
using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Views;

public class WorldRenderer
{
    private World _world;

    private const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    private const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;

    public WorldRenderer(World world)
    {
        _world = world;

        Console.WindowHeight = Constants.WORLD_SIZE.y + 1;
        Console.WindowWidth = Constants.WORLD_SIZE.x + 1;

        FConsole.Initialize("Rogue Project", FOREGROUND_COLOR, BACKGROUND_COLOR);
    }

    public void RenderWorld()
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var entity = _world.Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);

                if (entity != null)
                {
                    FConsole.SetChar(x, y, '@', ConsoleColor.Yellow, BACKGROUND_COLOR);
                }
                else
                {
                    var cell = _world.GetCell(new Vector2Int(x, y));

                    if (!cell.DoRender())
                    {
                        FConsole.SetChar(x, y, ' ', FOREGROUND_COLOR, BACKGROUND_COLOR);
                        continue;
                    }

                    var tileType = cell.TileType;
                    var tileChar = GetTileChar(tileType);
                    FConsole.SetChar(x, y, tileChar, FOREGROUND_COLOR, BACKGROUND_COLOR);
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
            _ => '?'
        };
    }
}
