using System.Text.Json;
using RogueProject.Utils;

namespace RogueProject.Models.Entities;

public class Enemy(string name, Vector2Int position) : Entity(name, position)
{
    public int Experience { get; private set; }
    public int Gold { get; private set; }

    public Enemy Clone(Vector2Int position)
    {
        return new Enemy(Name, position)
        {
            Health = Health,
            MaxHealth = MaxHealth,
            Strength = Strength,
            Armor = Armor,
            Character = Character,
            Color = Color
        };
    }

    protected override void LoadStats()
    {
        base.LoadStats();

        var jsonString = File.ReadAllText($"Data/Entities/{Name}.json");
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

        Experience = json["Experience"].ToInt();
        Gold = json["Gold"].ToInt();
    }

    public void ScaleStats(int floorNumber)
    {
        var scaleFactor = 1 + floorNumber * 0.15f;

        MaxHealth = (int)(MaxHealth * scaleFactor);
        Health = MaxHealth;

        Strength = (int)(Strength * scaleFactor);
        Armor = (int)(Armor * scaleFactor);

        Experience = (int)(Experience * scaleFactor);
        Gold = (int)(Gold * scaleFactor);
    }
}
