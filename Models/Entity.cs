﻿using System.Text.Json;
using RogueProject.Utils;

namespace RogueProject.Models;

public abstract class Entity
{
    protected readonly string Name;
    public Vector2Int Position;

    public int Health = -1;
    public int MaxHealth = -1;

    public int Strength = -1;
    public int Armor = -1;

    public char Character;
    public ConsoleColor Color;

    protected Entity(string name, Vector2Int position)
    {
        Name = name;
        Position = position;

        LoadStats();
    }

    private void LoadStats()
    {
        var jsonString = File.ReadAllText($"Data/Entities/{Name}.json");
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

        MaxHealth = json["MaxHealth"].ToInt();
        Health = MaxHealth;

        Strength = json["Strength"].ToInt();
        Armor = json["Armor"].ToInt();

        Character = json["Character"][0];

        Color = (ConsoleColor)json["Color"].ToInt();
    }
}
