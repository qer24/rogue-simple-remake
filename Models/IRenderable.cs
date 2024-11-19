namespace RogueProject.Models;

public interface IRenderable
{
    public char Character { get; }
    public ConsoleColor Color { get; }

    public bool IsVisible(World world);
}
