using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class Unit : Entity
{
    //Egységek osztálya.
    public Castle OpponentCastle { get; private set; } //Az ellenség kastélya, a legrövidebb útkereső algoritmushoz.
    public int Speed { get; protected set; } //Sebesség.
    public Queue<Tile> Path { get; private set; } //A legrövidebb út.
    public int Distance { get { return Path.Count; } } //Kastélytól való távolság.
    public float Number { get; set; } //Sorszám, adott mezőn hányadik.


    public Unit(Tile tile, Faction faction) : base(tile, faction) ///Konstruktor.
    {
    }
    
    public override void Update() //Frissítéssel lépés.
    {
        if (Faction == Faction.Alliance) //Factiontól függően legrövidebb út lekérdezése.
        {
            OpponentCastle = World.Current.Players[1].Castle;
            Path = PathFinder.FindPath(Tile, OpponentCastle.Tile);
        }
        else
        {
            OpponentCastle = World.Current.Players[0].Castle;
            Path = PathFinder.FindPath(Tile, OpponentCastle.Tile);
        }

        if (Path != null) //léptetés.
        {
            Path.Dequeue();

            for (var i = 0; i < Speed && Tile != OpponentCastle.Tile; ++i)
            {
                --Tile.OnUnit;
                Tile = Path.Dequeue();
                ++Tile.OnUnit;
                this.Number = Tile.OnUnit;
            }

            if(Tile == OpponentCastle.Tile)
            {
                --Tile.OnUnit;
                OpponentCastle.Health -= Damage;
                OpponentCastle.OnChange();
                World.Current.Units.Remove(this);
            }

            OnChange();
        }
        else
        {
            --Tile.OnUnit;
            World.Current.Units.Remove(this);
            OnChange();
        }
    }
}

public sealed class SimpleUnit : Unit //Simple Unit konstruktor adott paraméterekkel.
{
    public SimpleUnit(Tile tile, Faction faction) : base(tile, faction)
    {
        Health = 24;
        Damage = 5;
        Price = 10;
        Speed = 2;
        Number = 1;
    }
    
}

public sealed class StrongUnit : Unit //Strong Unit konstruktor adott paraméterekkel.
{
    public StrongUnit(Tile tile, Faction faction) : base(tile, faction)
    {
        Health = 44;
        Damage = 10;
        Price = 20;
        Speed = 1;
        Number = 1;
    }
}

