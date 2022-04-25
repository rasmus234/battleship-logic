using System.Numerics;

namespace gamelogic;

public class GameGrid
{
    public List<Ship> Ships { get; set; } = new();

    public bool AllSunk => Ships.All(ship => ship.IsSunk());

    public void PlaceShip(Ship ship)
    {
        Ships.Add(ship);
    }

    public void RecieveHit(Vector2 coordinate)
    {
        foreach (var ship in Ships.Where(ship => !ship.IsSunk()))
        {
            if (ship.Tiles.Contains(coordinate))
            {
                Console.Out.WriteLine("hit");
                ship.Hit(coordinate);
            }
        }
    }
    
    
    
    
    
}