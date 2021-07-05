using System;
using System.Collections.Generic;

//Gráf éleinek osztálya
public class Node
{
    public List<Edge> Edges; //Belőle kimenő élek
    public Tile Tile; //Tile, amihez hozzá van kötve

    public Node(Tile tile)
    {
        Tile = tile;
    }
}