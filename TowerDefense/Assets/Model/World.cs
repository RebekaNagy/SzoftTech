using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using UnityEngine.UI;

public class World : IDisplayable
{
    public static World Current { get; private set; } //Aktuális játékvilág.

    public Player ActualPlayer { get; set; } //Aktuális játékos.

    public int Size { get; private set; } //Játéktábla méret.

    public Tile[,] Tiles { get; set; } //Mezők mátrixa.

    public Player[] Players { get; private set; } //Játékosokat kételemű tömbben tároljuk.

    public List<Unit> Units { get; private set; } //Egységek listája.

    public List<Building> Buildings { get; private set; } //Épületek listája.

    public Graph Graph { get; private set; } //Útkereső algoritmushoz szükséges gráf.

    public bool IsEnd = false; //Játék vége.
    public string EndingString { get; set; } //Játék végén megjelenő szöveg.

    public bool CanStep { get; set; } //Léphet-e az adott játékos.

    public List<CommunicationModel.ServerEntity> NewEntity { get; set; }

    public static Network Network { get; set; }

    public bool OnPlaceEntity { get; set; }

    public bool OnEndTurn { get; set; }

    public bool NewRound { get; set; }

    public World() //Konstruktor.
    {
        Current = this;
    }

    public Tile this[int x, int y] //Tile mezők gettere.
    {
        get
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
            {
                return null;
            }
            return Tiles[x, y];
        }
    }

    public void NewGame() //Új játék.
    {
        EndingString = "oke"; //Null reference elkerülése a végző szöveggel.
         
        Size = 20;
        Tiles = new Tile[Size, Size]; //Tile-ok létrehozása.
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Tiles[i, j] = new Tile(i, j, Maps.ActualMap[i, j]);
            }
        }
        
        Players = new Player[2];

        //Játékosok létrehozása, a kastélyukkal és barakkjaikkal együtt.
        Players[0] = new Player(Faction.Alliance);
        Players[0].Castle = new Castle(Tiles[3,16], Faction.Alliance);
        Players[0].Barracks[0] = new Barrack(Tiles[9,18], Faction.Alliance);
        Players[0].Barracks[1] = new Barrack(Tiles[1,14], Faction.Alliance);

        Players[1] = new Player(Faction.Horde);
        Players[1].Castle = new Castle(Tiles[16,3], Faction.Horde);
        Players[1].Barracks[0] = new Barrack(Tiles[11,1], Faction.Horde);
        Players[1].Barracks[1] = new Barrack(Tiles[18,5], Faction.Horde);

        Units = new List<Unit>(); //Listák létrehozása.
        Buildings = new List<Building>();

        NewEntity = new List<CommunicationModel.ServerEntity>();

        //Lefoglalt tile-ok beállítása.
        Tiles[9, 18].Type = TileType.Occupied;
        Tiles[1, 14].Type = TileType.Occupied;
        Tiles[11, 1].Type = TileType.Occupied;
        Tiles[18, 5].Type = TileType.Occupied;

        Graph = new Graph(Current);

        //Kezdő épületek listába helyezése.
        Buildings.Add(Players[0].Castle);
        Buildings.Add(Players[1].Castle);
        Buildings.Add(Players[0].Barracks[0]);
        Buildings.Add(Players[0].Barracks[1]);
        Buildings.Add(Players[1].Barracks[0]);
        Buildings.Add(Players[1].Barracks[1]);

        if (Network.Current != null && Network.Current.GetPlayerOrder() == 2)
        {
            CanStep = false;
            ActualPlayer = Players[1];
        }
        else
        {
            CanStep = true;
            ActualPlayer = Players[0];
        }

        OnPlaceEntity = false;
        OnEndTurn = false;
        NewRound = true;

        OnChange();
    }

    public PlayerActionResult CreateTower(int x, int y, string tower) //Torony létrehozása.
    {
        var result = new PlayerActionResult //Alapvetően sikeresség igaz.
        {
            ErrorMessage = "Nincs eleg penzed!",
            Success = true
        };

        if(!CanStep)
        {
            result.ErrorMessage = "Az ellenfel lep!";
            result.Success = false;
            return result;
        }

        if(!Tiles[x, y].CanPlaceEntity ||
            PathFinder.FindPath(Players[0].Barracks[0].Tile, Players[1].Castle.Tile, Tiles[x, y]) == null ||
            PathFinder.FindPath(Players[0].Barracks[1].Tile, Players[1].Castle.Tile, Tiles[x, y]) == null ||
            PathFinder.FindPath(Players[1].Barracks[0].Tile, Players[0].Castle.Tile, Tiles[x, y]) == null ||
            PathFinder.FindPath(Players[1].Barracks[1].Tile, Players[0].Castle.Tile, Tiles[x, y]) == null )
        {
            result.ErrorMessage = "Torony nem helyezheto erre a feluletre!";
            result.Success = false;
            return result;
        }

        //A mousecontrollertől kapott stringtől függően, az adott torony létrehozása a kapott pozíciókon.
        if (tower == "SimpleTower") 
        {
            var newTower = new SimpleTower(Tiles[x, y], ActualPlayer.Faction);
            if (ActualPlayer.Coins - newTower.Price >= 0) //Elegendő pénzmennyiség.
            {
                AddNewEntity(x, y, tower);

                ActualPlayer.Coins = ActualPlayer.Coins - newTower.Price;
                Buildings.Add(newTower);
            }
            else
            {
                result.Success = false;
                return result;
            }
        }
        else if(tower == "RangeTower")
        {
            var newTower = new RangeTower(Tiles[x, y], ActualPlayer.Faction);
            if (ActualPlayer.Coins - newTower.Price >= 0)
            {
                AddNewEntity(x, y, tower);

                ActualPlayer.Coins = ActualPlayer.Coins - newTower.Price;
                Buildings.Add(newTower);
            }
            else
            {
                result.Success = false;
                return result;
            }
        }
        else if(tower == "CannonTower")
        {
            var newTower = new CannonTower(Tiles[x, y], ActualPlayer.Faction);
            if (ActualPlayer.Coins - newTower.Price >= 0)
            {
                AddNewEntity(x, y, tower);

                ActualPlayer.Coins = ActualPlayer.Coins - newTower.Price;
                Buildings.Add(newTower);
            }
            else
            {
                result.Success = false;
                return result;
            }
        }

        if (result.Success)
        {
            Tiles[x, y].Type = TileType.Occupied;
            Graph.RecreateEdges(Tiles[x, y]);
            OnChange();
        }

        return result;
    }

    public PlayerActionResult CreateUnit(string unit) //Egységek létrehozása.
    {
        var result = new PlayerActionResult //Alapvetően  sikeresség igaz.
        {
            ErrorMessage = "Nincs eleg penzed!",
            Success = true
        };

        if (!CanStep)
        {
            result.ErrorMessage = "Az ellenfel lep!";
            result.Success = false;
            return result;
        }

        Random random = new Random();
        var position = random.Next(2);
        //A mousecontrollertől kapott stringtől függően, az adott egység egy saját barakk pozícióján, ez pedig randomizálva van.
        if (unit == "SimpleUnit")
        {
            var newUnit = new SimpleUnit(ActualPlayer.Barracks[position].Tile, ActualPlayer.Faction);
            if (ActualPlayer.Coins - newUnit.Price >= 0)
            {
                AddNewEntity(ActualPlayer.Barracks[position].X, ActualPlayer.Barracks[position].Y, unit);

                ++ActualPlayer.Barracks[position].Tile.OnUnit;
                ActualPlayer.Coins = ActualPlayer.Coins - newUnit.Price;
                Units.Add(newUnit);
            }
            else
            {
                result.Success = false;
                return result;
            }
        }
        else if (unit == "StrongUnit")
        {
            var newUnit = new StrongUnit(ActualPlayer.Barracks[position].Tile, ActualPlayer.Faction);
            if (ActualPlayer.Coins - newUnit.Price >= 0)
            {
                AddNewEntity(ActualPlayer.Barracks[position].X, ActualPlayer.Barracks[position].Y, unit);

                ++ActualPlayer.Barracks[position].Tile.OnUnit;
                ActualPlayer.Coins = ActualPlayer.Coins - newUnit.Price;
                Units.Add(newUnit);
            }
            else
            {
                result.Success = false;
                return result;
            }
        }

        if (result.Success)
        {
            OnChange();
        }

        return result;
    }

    public void Turn() //Kör vége.
    {
        if (ActualPlayer == Players[0])
        {
            ActualPlayer = Players[1];
        }
        else
        {
            EndTurn();
            ActualPlayer = Players[0];
        }

        OnChange();
    }

    public void EndTurn()
    {
        for (var i = Units.Count - 1; i >= 0; --i)
        {
            Units[i].Update();
        }

        if (Players[0].Castle.Health <= 0 && Players[1].Castle.Health <= 0)
        {
            EndingString = "Draw.";
            IsEnd = true;
            if (Network.Current != null)
            {
                Network.Current.ExitGame();
                Network.Current = null;
            }
            return;
        }
        else if (Players[0].Castle.Health <= 0)
        {
            EndingString = "Win:Horde";
            IsEnd = true;
            if (Network.Current != null)
            {
                Network.Current.ExitGame();
                Network.Current = null;
            }
            return;
        }
        else if (Players[1].Castle.Health <= 0)
        {
            EndingString = "Win:Alliance";
            IsEnd = true;
            if (Network.Current != null)
            {
                Network.Current.ExitGame();
                Network.Current = null;
            }
            return;
        }

        foreach (var tower in Buildings)
        {
            if (tower.Damage > 0) tower.Update();
        }

        for (var i = Units.Count - 1; i >= 0; --i)
        {
            Units[i].OnChange();
        }

        foreach (var unit in Units)
        {
            if (unit.Faction == Faction.Alliance)
            {
                Players[0].Coins += unit.Price / 10;
            }
            else
            {
                Players[1].Coins += unit.Price / 10;
            }
        }

        if (Network.Current != null)
        {
            Network.Current.SetEndOfRound();
        }
    }

    public void ServerTurn()
    {
        if (!CanStep) return;

        CanStep = false;

        Network.Current.SendStep(NewEntity);
        NewEntity.Clear();
    }

    public void PlaceEntity(List<CommunicationModel.ServerEntity> list) //Szerverbe feltöltött entity-k lehelyezése
    {
        Faction opponentFaction;
        if (ActualPlayer.Faction == Faction.Alliance)
        {
            opponentFaction = Faction.Horde;
        }
        else
        {
            opponentFaction = Faction.Alliance;
        }

        foreach(var entity in list)
        {
            if (entity.EntityType == "SimpleTower")
            {
                var newTower = new SimpleTower(Tiles[entity.X, entity.Y], opponentFaction);
                Tiles[entity.X, entity.Y].Type = TileType.Occupied;
                Graph.RecreateEdges(Tiles[entity.X, entity.Y]);
                Buildings.Add(newTower);
            }
            else if (entity.EntityType == "RangeTower")
            {
                var newTower = new RangeTower(Tiles[entity.X, entity.Y], opponentFaction);
                Tiles[entity.X, entity.Y].Type = TileType.Occupied;
                Graph.RecreateEdges(Tiles[entity.X, entity.Y]);
                Buildings.Add(newTower);
            }
            else if (entity.EntityType == "CannonTower")
            {
                var newTower = new CannonTower(Tiles[entity.X, entity.Y], opponentFaction);
                Tiles[entity.X, entity.Y].Type = TileType.Occupied;
                Graph.RecreateEdges(Tiles[entity.X, entity.Y]);
                Buildings.Add(newTower);
            }
            else if (entity.EntityType == "SimpleUnit")
            {
                var newUnit = new SimpleUnit(Tiles[entity.X, entity.Y], opponentFaction);
                ++Tiles[entity.X, entity.Y].OnUnit;
                Units.Add(newUnit);
            }
            else if (entity.EntityType == "StrongUnit")
            {
                var newUnit = new StrongUnit(Tiles[entity.X, entity.Y], opponentFaction);
                ++Tiles[entity.X, entity.Y].OnUnit;
                Units.Add(newUnit);
            }
            OnChange();
        }
    }

    private void AddNewEntity(int x, int y, string type)
    {
        if (Network.Current != null)
        {
            CommunicationModel.ServerEntity newEntity = new CommunicationModel.ServerEntity
            {
                X = x,
                Y = y,
                EntityType = type
            };
            NewEntity.Add(newEntity);
        }
    }

    public event Action Changed;
    public int X => 0;
    public int Y => 0;
    public void OnChange()
    {
        Changed?.Invoke();
    }
}
