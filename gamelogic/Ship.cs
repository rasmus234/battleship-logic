using System.Numerics;

namespace gamelogic;

public class Ship
{
    public Vector2[] Tiles { get; set; } = new Vector2[10];
    public List<Vector2> TilesHit { get; set; } = new List<Vector2>();


    public Ship()
    {
        
    }

    public void Hit(Vector2 tile)
    {
        TilesHit.Add(tile);
    }

    public bool IsSunk()
    {
        if (Tiles.All(tile => TilesHit.Contains(tile))) return true;
        return false;

    }
}