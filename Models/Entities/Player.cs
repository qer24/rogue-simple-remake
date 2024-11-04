using RogueProject.Utils;

namespace RogueProject.Models.Entities;

public class Player(string name, Vector2Int position) : Entity(name, position)
{
    public int Level = 1;
    public int Experience = 0;
    public int ExperienceToNextLevel = 10;

    public int Gold = 0;

    public void LevelUp()
    {
        Level++;
        Experience = 0;
        ExperienceToNextLevel *= (int)MathF.Floor(ExperienceToNextLevel * 1.2f);

        MaxHealth += 5;
        Health = MaxHealth;

        Strength += 1;
        Armor += 1;

        Logger.Log($"{Name} has leveled up! New stats: Health: {MaxHealth}, Strength: {Strength}, Armour: {Armor}");
    }
}
