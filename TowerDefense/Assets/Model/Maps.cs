using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Maps
{   
    //World térképeinek osztálya.
    public static TileType[,] ActualMap { get; set; } //Worldben csak a tile elérhetőségét tároljuk, más nem fontos.

    public static void Initialize(string map) //Inicializáció
    {
        switch (map)
        {
            case "elso":
                ActualMap = map1;
                break;
            case "masodik":
                ActualMap = map2;
                break;
            case "harmadik":
                ActualMap = map3;
                break;
        }
    }

    public static TileType[,] map1 = new TileType[,] //térképek
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    };

    public static TileType[,] map2 = new TileType[,]
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied},
        {0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied},
        {0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied},
        {0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied},
        {0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied},
    };

    public static TileType[,] map3 = new TileType[,]
    {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0},
        {TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0},
        {TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, TileType.Occupied, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, TileType.Occupied, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0},
        {0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0},
        {0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, TileType.Occupied, TileType.Occupied, 0, 0, 0, 0, 0, 0, 0},
    };
}

