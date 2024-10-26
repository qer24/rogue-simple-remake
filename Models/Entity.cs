using ProjektFB.Utils;

namespace ProjektFB.Models;

public class Entity(string name, Vector2Int position)
{
    public string Name = name;
    public Vector2Int Position = position;
}
