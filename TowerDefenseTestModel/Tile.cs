using System;
using System.Collections.Generic;

public class Tile : IDisplayable
{
    //Tile-ok osztálya, a térkép mezői.
    public int X { get; } 
    //Koordináták.
    public int Y { get; }

    public TileType Type { get; set; } //Van-e rajta valami, vagy sem.
    
    public int OnUnit { get; set; } //Mezőn található Unitok száma.

    public bool CanPlaceEntity => Type == TileType.Enterable && OnUnit == 0; //Ráhelyezhető-e torony/egység.

    public Tile(int x, int y, TileType type) //Konstruktor.
    {
        X = x;
        Y = y;
        Type = type;
    }

    public event Action Changed;
    public void OnChange() // Call every time the view needs to be updated
    {
        Changed?.Invoke();
    }
}
