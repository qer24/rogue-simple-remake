using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueProject.Models;

public static class EnemyDatabase
{
    private static Enemy[] _enemies;

    /// <summary>
    /// All enemy types in the game.
    /// </summary>
    public static Enemy[] Enemies => _enemies ??= LoadEnemies();

    private static Enemy[] LoadEnemies()
    {
        var enemyFiles = Directory.GetFiles("Data/Entities");

        List<Enemy> list = [];

        foreach (string fileName in enemyFiles.Select(Path.GetFileNameWithoutExtension))
        {
            if (fileName == "Player")
            {
                continue;
            }
            list.Add(new Enemy(fileName, Vector2Int.zero));
        }

        return list.ToArray();
    }
}
