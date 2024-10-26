using ProjektFB.Models;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ProjektFB;

public static class Program
{
    public static void Main()
    {
        var worldGrid = new WorldCell[Constants.WORLD_SIZE.x, Constants.WORLD_SIZE.y];

        var grid = new Grid();

        for (int x = 0; x < Constants.WORLD_SIZE.x; x++)
        {
            grid.AddColumn();
        }

        for (int y = 0; y < Constants.WORLD_SIZE.y; y++)
        {
            var row = new string[Constants.WORLD_SIZE.x];
            for (int i = 0; i < row.Length; i++)
            {
                row[i] = "@";
            }
            grid.AddRow(row);
        }

        AnsiConsole.Write(grid);

        Console.ReadKey();
    }
}
