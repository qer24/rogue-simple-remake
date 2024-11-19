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

    public const int MIN_ITEMS_PER_ROOM = 0;
    public const int MAX_ITEMS_PER_ROOM = 2;

    public const int MAX_ENEMIES_PER_ROOM = 1;
    public const int MIN_ENEMIES_PER_ROOM = 0;

    // World visibility
    public const int FLOOR_REVEAL_DISTANCE = 6;

    // Fighting
    public const float ARMOR_SCALING_FACTOR = 0.06f; // Each point of armor reduces damage by 6%
    public const float MAX_DAMAGE_REDUCTION = 0.75f; // Cap damage reduction at 75%
    public const float PENETRATION_FACTOR = 0.5f;    // How much high strength can pierce armor
}
