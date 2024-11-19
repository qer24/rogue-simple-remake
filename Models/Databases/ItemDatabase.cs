using RogueProject.Utils;

namespace RogueProject.Models;

public static class ItemDatabase
{
    private static Item[] _items;

    /// <summary>
    /// All item types in the game.
    /// </summary>
    public static Item[] Items => _items ??= LoadItems();

    private static Item[] LoadItems()
    {
        var itemFiles = Directory.GetFiles("Data/Items");

        return itemFiles.Select(Path.GetFileNameWithoutExtension)
                        .Select(fileName => new Item(fileName, Vector2Int.zero))
                        .ToArray();
    }
}
