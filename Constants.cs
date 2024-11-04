using RogueProject.Utils;

namespace RogueProject;

public static class Constants
{
    public static readonly Vector2Int WORLD_SIZE = new(80, 24);

    public const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    public const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;

    // World generation
    public const int MAX_GONE_ROOMS = 3;
    public const int RANDOM_CONNECTION_COUNT = 3;

    // World visibility
    public const int FLOOR_REVEAL_DISTANCE = 6;
}
