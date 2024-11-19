using System.Text.Json;
using RogueProject.Models.Entities;
using RogueProject.Utils;

namespace RogueProject.Models;

public class Item : IRenderable
{
    public readonly string Name;
    public Vector2Int Position;

    public char Character { get; private set; }
    public ConsoleColor Color { get; private set; }
    public string PickupMessage;

    private Action<Player>[] _effects;

    public Item(string name, Vector2Int position)
    {
        Name = name;
        Position = position;

        LoadStats();
    }

    public Item Clone(Vector2Int position)
    {
        return new Item(Name, position);
    }

    /// <summary>
    /// Load stats from json file associated with item.
    /// </summary>
    private void LoadStats()
    {
        var jsonString = File.ReadAllText($"Data/Items/{Name}.json");
        var json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString);

        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;

        Character = json["Character"].GetString()![0];
        Color = (ConsoleColor)json["Color"].GetString().ToInt();

        PickupMessage = json["Message"].GetString();

        if (!json.TryGetValue("Effects", out var effects))
        {
            return;
        }

        Logger.Log(effects.GetType().ToString());

        var effectsList = new List<Action<Player>>();

        foreach (var effect in effects.EnumerateObject())
        {
            string key = effect.Name;
            if (!int.TryParse(effect.Value.GetString(), out int amount))
            {
                continue;
            }

            Action<Player> action = key switch
            {
                "Health"     => player => player.ChangeHealth(amount),
                "Strength"   => player => player.Strength += amount,
                "Armor"      => player => player.Armor += amount,
                "Gold"       => player => player.Gold += amount,
                "Experience" => player => player.AddExperience(amount),
                _            => null
            };

            if (action != null)
            {
                effectsList.Add(action);
            }
            else
            {
                Logger.Log($"Warning: Unknown effect type '{key}' in item {Name}");
            }
        }

        _effects = effectsList.ToArray();
    }

    public void ApplyEffect(Player player)
    {
        foreach (var effect in _effects)
        {
            effect(player);
        }
    }
}
