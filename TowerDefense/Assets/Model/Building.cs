using System.Collections.Generic;
using System.Linq;

public abstract class Building : Entity
{
    //Épület osztály.
    public int Range { get; protected set; }
    //Tornyok lőtávja.
    protected Building(Tile tile, Faction faction) : base(tile, faction)
    {
    }//Ősosztály konstruktor.

    public IEnumerable<Tile> TilesInRange() //Tornyok lőtávjába tartozó tileok.
    {
        var inRange = new List<Tile>();

        for (var i = -Range; i <= Range; i++)
        {
            for (var j = -Range; j <= Range; j++)
            {
                inRange.Add(World.Current[Tile.X + i, Tile.Y + j]);
            }
        }

        return inRange.Where(t => t != null);
    }
}

public sealed class Barrack : Building //Barakk osztály
{
    // spawn unit method
    public Barrack(Tile tile, Faction faction) : base(tile, faction) //Csak pozíciója van
    {
    }

    public override void Update()
    {
        //do nothing?
        throw new System.NotImplementedException();
    }
}

public sealed class Castle : Building //Kastély osztály.
{
    public Castle(Tile tile, Faction faction) : base(tile, faction) //Pozíciója és életereje van.
    {
        Health = 200;
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

public abstract class Tower : Building //Torony osztály.
{
    public virtual void Shoot() //Egy lövés metódusa van, ami a lőtávjába kerülő egységek közül kiválasztja a listában legelöl 
    {                           //szereplő unitot, és azt lövi.
        var InRange = TilesInRange();
        Unit minUnit = null;

        foreach (var tile in InRange)
        {
            if (tile.OnUnit != 0)
            {
                var found = false;

                for(var i = 0; i < World.Current.Units.Count && !found; ++i)
                {
                    if (World.Current.Units[i].Tile == tile && World.Current.Units[i].Faction != Faction && minUnit == null)
                    {
                        minUnit = World.Current.Units[i];
                    }

                    else if (World.Current.Units[i].Tile == tile && World.Current.Units[i].Faction != Faction && World.Current.Units[i].Distance < minUnit.Distance)
                    {
                        minUnit = World.Current.Units[i];
                    }

                    if (World.Current.Units[i].Tile == tile)
                    {
                        found = true;
                    }
                }
            }
        }

        if (minUnit != null)
        {
            minUnit.Health -= Damage;

            if (minUnit.Health <= 0)
            {
                --minUnit.Tile.OnUnit;

                if (Faction == Faction.Alliance)
                {
                    World.Current.Players[0].Coins += minUnit.Price / 2;
                }
                else
                {
                    World.Current.Players[1].Coins += minUnit.Price / 2;
                }
                World.Current.Units.Remove(minUnit);
            }
        }
    }

    public Tower(Tile tile, Faction faction) : base(tile, faction) //Csak pozíció.
    {
    }

    public override void Update()
    {
        Shoot();
    }
}

public sealed class SimpleTower : Tower //Simple Tower osztály, megadott paraméterekkel, a shoot műveletét a bázisosztálytól kapja.
{
    public SimpleTower(Tile tile, Faction faction) : base(tile, faction)
    {
        Range = 1;
        Damage = 5;
        Price = 10;
    }
}

public sealed class RangeTower : Tower //Range Tower osztály, megadott paraméterekkel, a shoot műveletét a bázisosztálytól kapja.
{
    public RangeTower(Tile tile, Faction faction) : base(tile, faction)
    {
        Range = 3;
        Damage = 3;
        Price = 15;
    }
}


public sealed class CannonTower : Tower//Simple Tower osztály, megadott paraméterekkel, a shoot műveletét módosítjuk, mivel nem egy unitot lő
{                                       //hanem egy egész tile-t.
    public CannonTower(Tile tile, Faction faction) : base(tile, faction)
    {
        Range = 1;
        Damage = 5;
        Price = 20;
    }

    public override void Shoot()
    {
        var InRange = TilesInRange();
        int minDistance = World.Current.Size*2;
        Tile minTile = null;

        foreach (var tile in InRange)
        {
            if (tile.OnUnit != 0)
            {
                var found = false;

                for (var i = 0; i < World.Current.Units.Count && !found; ++i)
                {
                    if (World.Current.Units[i].Tile == tile && World.Current.Units[i].Faction != Faction && World.Current.Units[i].Distance < minDistance)
                    {
                        minDistance = World.Current.Units[i].Distance;
                        minTile = tile;
                    }

                    if (World.Current.Units[i].Tile == tile) found = true;
                }
            }
        }

        if (minTile != null)
        {
            for (var i = World.Current.Units.Count - 1; i >= 0; --i)
            {
                if (World.Current.Units[i].Tile == minTile && World.Current.Units[i].Faction != Faction)
                {
                    World.Current.Units[i].Health -= Damage;

                    if (World.Current.Units[i].Health <= 0)
                    {
                        --World.Current.Units[i].Tile.OnUnit;

                        if (Faction == Faction.Alliance)
                        {
                            World.Current.Players[0].Coins += World.Current.Units[i].Price / 2;
                        }
                        else
                        {
                            World.Current.Players[1].Coins += World.Current.Units[i].Price / 2;
                        }
                        World.Current.Units.Remove(World.Current.Units[i]);
                    }
                }
            }
        }
    }
}
