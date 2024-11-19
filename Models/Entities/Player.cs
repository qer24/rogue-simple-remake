using RogueProject.Utils;

namespace RogueProject.Models.Entities;

public class Player(string name, Vector2Int position) : Entity(name, position)
{
    public int Level { get; private set; } = 1;
    public int Experience { get; private set; }
    public int ExperienceToNextLevel { get; private set; } = 10;

    public int Gold;

    public override bool IsVisible(World world)
    {
        return true;
    }

    public void AddExperience(int amount)
    {
        Experience += amount;
        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        Experience = 0;
        ExperienceToNextLevel = (int)MathF.Floor(ExperienceToNextLevel * 1.2f);

        MaxHealth += 5;
        Health = MaxHealth;

        Strength += 1;
        Armor += 1;

        Logger.Log($"{Name} has leveled up! New stats: Health: {MaxHealth}, Strength: {Strength}, Armour: {Armor}");
        UiMessage.Instance.ShowMessage("                Level up!", 5, true);
    }
}
