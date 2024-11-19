using FastConsole;
using RogueProject.Controllers;
using RogueProject.Models;
using RogueProject.Models.Entities;
using RogueProject.Utils;
using RogueProject.Views;

namespace RogueProject;

public static class Program
{
    // ReSharper disable once FunctionRecursiveOnAllPaths
    public static void Main(string[] args)
    {
        Console.WindowHeight = Constants.WORLD_SIZE.y + 2;
        Console.WindowWidth = Constants.WORLD_SIZE.x + 1;

        FConsole.Initialize("Rogue Project", Constants.FOREGROUND_COLOR, Constants.BACKGROUND_COLOR);

        if (!args.Contains("skipintro"))
        {
            Intro();
        }

        var world = new World();
        var worldController = new WorldController(world);
        var worldRenderer = new WorldRenderer(world);

        UiMessage.Instance.Reset();

        var player = world.Entities[0] as Player;
        var playerController = new PlayerController(world, player);
        var uiRenderer = new UiRenderer(player);

        var endGame = false;

        void Update(bool firstUpdate = false)
        {
            if (!firstUpdate)
                playerController.Update();
            worldController.Update();

            if (worldController.PlayerDead)
            {
                endGame = true;
            }

            worldRenderer.Render();
            uiRenderer.Render();

            FConsole.DrawBuffer();
        }

        // force first update before players makes any input
        Update(true);

        while (!endGame)
        {
            Update();
        }

        Outro(world.Player.Level, world.Player.Gold);
        Main(args);
    }

    private static void Intro()
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

    private static void Outro(int level, int gold)
    {
        string[] asciiArt =
        [
            @"  ____                         ___                 ",
            @" / ___| __ _ _ __ ___   ___  / _ \__   _____ _ __",
            @"| |  _ / _` | '_ ` _ \ / _ \| | | \ \ / / _ \ '__|",
            @"| |_| | (_| | | | | | |  __/| |_| |\ V /  __/ |   ",
            @" \____|\__,_|_| |_| |_|\___| \___/  \_/ \___|_|   "
        ];

        // Calculate center position for the ASCII art
        int windowWidth = Console.WindowWidth;
        int windowHeight = Console.WindowHeight;
        int artWidth = asciiArt[0].Length;
        int startCol = (windowWidth - artWidth) / 2;
        int startRow = (windowHeight - asciiArt.Length) / 2 - 4;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;

        // Print ASCII art centered
        for (int i = 0; i < asciiArt.Length; i++)
        {
            Console.SetCursorPosition(startCol, startRow + i);
            Console.WriteLine(asciiArt[i]);
        }

        // Print stats messages
        Console.ForegroundColor = ConsoleColor.Yellow;
        string levelMsg = $"Level Reached: {level}";
        string goldMsg = $"Gold Collected: {gold}";

        Console.SetCursorPosition((windowWidth - levelMsg.Length) / 2, startRow + asciiArt.Length + 2);
        Console.WriteLine(levelMsg);

        Console.SetCursorPosition((windowWidth - goldMsg.Length) / 2, startRow + asciiArt.Length + 3);
        Console.WriteLine(goldMsg);

        // Print exit message
        Console.ForegroundColor = ConsoleColor.White;
        const string message = "Press Any Key to Restart";
        Console.SetCursorPosition((windowWidth - message.Length) / 2, startRow + asciiArt.Length + 5);
        Console.WriteLine(message);

        Console.ResetColor();
        Console.ReadKey();
        Console.Clear();
    }
}
