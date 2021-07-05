using System;

public abstract class Entity : IDisplayable
{
    //Entitások(torony, egység) ősosztálya.
    public Tile Tile { get; set; } //Hozzá tartozó tile.

    public World World { get; }//Adott Worldhoz tartozik.

    public Faction Faction { get; } //Alliance/Horde

    public int Health { get; set; } //Életerő.

    public int Damage { get; set; } //Sebzés.

    public int Price { get; set; } //Ár.
    
    protected Entity(Tile tile, Faction faction) //Konstruktor.
    {
        World = World.Current;
        Tile = tile;
        Faction = faction;
    }

    public abstract void Update(); //Frissítés.

    public event Action Changed;

    //IDisplayable-hös szükséges. 
    public int X => Tile.X;

    public int Y => Tile.Y;

    public void OnChange()
    {
        Changed?.Invoke();
    }
}