using FastConsole;
using RogueProject.Models;
using RogueProject.Utils;

namespace RogueProject.Views;

public class WorldRenderer : Renderer
{
    private readonly World _world;

    public WorldRenderer(World world)
    {
        _world = world;

        Console.WindowHeight = Constants.WORLD_SIZE.y + 2;
        Console.WindowWidth = Constants.WORLD_SIZE.x + 1;

        FConsole.Initialize("Rogue Project", Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);
    }

    public override void Render()
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
                    FConsole.SetChar(x, y, '@', ConsoleColor.Yellow, Constants.BACKGROUND_COLOR);
                }
                else
                {
                    var cell = _world.GetCell(new Vector2Int(x, y));

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
            _ => '?'
        };
    }
}
