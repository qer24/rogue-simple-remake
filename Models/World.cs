using RogueProject.Controllers;
using RogueProject.Views;
using RogueProject.Utils;

namespace RogueProject.Models;

public class World : Singleton<World>
{
    public Room[] Rooms;
    public WorldCell[,] WorldGrid;
    public List<Entity> Entities;

    public WorldCell GetCell(Vector2Int position)
    {
        return WorldGrid[position.x, position.y];
    }

    public void RevealRoom(Room room)
    {
        var x = room.Position.x;
        var y = room.Position.y;
        var width = room.Size.x;
        var height = room.Size.y;

        for (var i = x; i < x + width; i++)
        {
            for (var j = y; j < y + height; j++)
            {
                WorldGrid[i, j].Revealed = true;
            }
        }
    }
}
