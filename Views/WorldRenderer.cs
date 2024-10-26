using ProjektFB.Models;
using Spectre.Console;

namespace ProjektFB.Views;

public static class WorldRenderer
{
    public static void RenderWorld(World world)
    {
        AnsiConsole.Clear();

        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        var table = new Table();

        for (int x = 0; x < sizeX; x++)
        {
            table.AddColumn("");
        }

        for (int y = 0; y < sizeY; y++)
        {
            var row = new string[sizeX];
            for (int x = 0; x < row.Length; x++)
            {
                var entity = world.Entities.FirstOrDefault(e => e.Position.x == x && e.Position.y == y);

                if (entity != null)
                {
                    row[x] = "@";
                }
                else if (x == 0 || x == sizeX - 1 || y == 0 || y == sizeY - 1)
                {
                    row[x] = ".";
                }
                else
                {
                    row[x] = " ";
                }
            }
            table.AddRow(row);
        }

        table.HideHeaders();
        table.Border(TableBorder.None);
        table.HideRowSeparators();
        table.Alignment(Justify.Center);

        AnsiConsole.Write(table);
    }
}
