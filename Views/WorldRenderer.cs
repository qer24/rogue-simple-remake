using RogueProject.Models;
using RogueProject.Utils;
using Spectre.Console;

namespace RogueProject.Views;

public class WorldRenderer
{
    public WorldRenderer()
    {
        AnsiConsole.Cursor.Hide();
        AnsiConsole.Clear();

        // Set the size of the console window
        Console.WindowHeight = Constants.WORLD_SIZE.y + 1;
        Console.WindowWidth = Constants.WORLD_SIZE.x + 1;
    }

    public void RenderWorld(World world)
    {
        AnsiConsole.Clear();

        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        var grid = new string[sizeY, sizeX];

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var entity = world.Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);

                if (entity != null)
                {
                    grid[y, x] = "@";
                }
                else
                {
                    var tileType = world.GetCell(new Vector2Int(x, y)).TileType;
                    grid[y, x] = GetTileChar(tileType).ToString();
                }
            }
        }

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                AnsiConsole.Write(new Markup(grid[y, x]));
            }
            AnsiConsole.WriteLine();
        }
    }

    private static char GetTileChar(TileType tileType)
    {
        return tileType switch
        {
            TileType.Floor => '.',
            TileType.WallTop => '_',
            TileType.WallBottom => '\u00af',
            TileType.WallVertical => '|',
            TileType.Empty => ' ',
            _ => '?'
        };
    }
}
