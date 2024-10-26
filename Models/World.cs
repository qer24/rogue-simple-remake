using ProjektFB.Controllers;
using ProjektFB.Utils;
using ProjektFB.Views;

namespace ProjektFB.Models;

// Singleton przechowujacy swiat gry
public class World : Singleton<World>
{
    public WorldCell[,] WorldGrid;
    public List<Entity> Entities;
}
