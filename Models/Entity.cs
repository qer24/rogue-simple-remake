using RogueProject.Utils;

namespace RogueProject.Models;

public class Entity(string name, Vector2Int position)
{
    public string Name = name;
    public Vector2Int Position = position;
}
