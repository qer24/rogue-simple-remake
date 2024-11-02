using RogueProject.Controllers;
using RogueProject.Views;
using RogueProject.Utils;

namespace RogueProject.Models;

public class World : Singleton<World>
{
    public WorldCell[,] WorldGrid;
    public List<Entity> Entities;

    public WorldCell GetCell(Vector2Int position)
    {
        return WorldGrid[position.x, position.y];
    }
}
