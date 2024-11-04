namespace RogueProject.Utils;

public static class RandomExtensions
{
    public static int Range(this System.Random rng, int min, int max) => rng.Next(min, max);
}
