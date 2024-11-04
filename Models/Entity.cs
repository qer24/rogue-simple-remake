using RogueProject.Utils;

namespace RogueProject.Models;

public class Entity(string name, Vector2Int position)
{
    public string Name = name;
    public Vector2Int Position = position;

    public int Health = 10;
    public int MaxHealth = 10;

    public int Strength = 10;
    public int Armor = 10;
}
