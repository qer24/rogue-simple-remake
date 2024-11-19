using FastConsole;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Utils;
using RogueProject.Views;

namespace RogueProject;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WindowHeight = Constants.WORLD_SIZE.y + 2;
        Console.WindowWidth = Constants.WORLD_SIZE.x + 1;

        FConsole.Initialize("Rogue Project", Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);

        if (!args.Contains("skipintro"))
        {
            // ASCII art for "ROGUE"
            string[] asciiArt =
            [
                @" ____                        ",
                @"|  _ \ ___   __ _ _   _  ___ ",
                @"| |_) / _ \ / _` | | | |/ _ \",
                @"|  _ < (_) | (_| | |_| |  __/",
                @"|_| \_\___/ \__, |\__,_|\___|",
                @"            |___/            "
            ];

            // Calculate center position for the ASCII art
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;
            int artWidth = asciiArt[0].Length;
            int startCol = (windowWidth - artWidth) / 2;
            int startRow = (windowHeight - asciiArt.Length) / 2 - 2;

            // Print ASCII art centered
            for (int i = 0; i < asciiArt.Length; i++)
            {
                Console.SetCursorPosition(startCol, startRow + i);
                Console.WriteLine(asciiArt[i]);
            }

            // Print "press enter to play" message
            Console.ForegroundColor = ConsoleColor.White;
            const string message = "Press Any Key to Play";
            Console.SetCursorPosition((windowWidth - message.Length) / 2, startRow + asciiArt.Length + 2);
            Console.WriteLine(message);

            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        var world = new World();
        var worldController = new WorldController(world);
        var worldRenderer = new WorldRenderer(world);

        var player = world.Entities[0] as Player;
        var playerController = new PlayerController(world, player);
        var uiRenderer = new UiRenderer(player);

        void Update(bool firstUpdate = false)
        {
            if (!firstUpdate)
                playerController.Update();
            worldController.Update();
            worldRenderer.Render();
            uiRenderer.Render();

            FConsole.DrawBuffer();
        }

        // force first update before players makes any input
        Update(true);

        while (true)
        {
            Update();
        }
    }
}
