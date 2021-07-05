using System;
using System.Collections.Generic;

//Gráf osztály, útkeresőhöz, irányott gráf
public class Graph
{
    public Dictionary<Tile, Node> Nodes;

    public Graph(World world)
    {
        if (world == null) return;

        Nodes = new Dictionary<Tile, Node>();

        foreach (var tile in world.Tiles)
        {
            var node = new Node(tile);

            Nodes.Add(tile, node);
        }

        foreach (var tile in Nodes.Keys) CreateEdges(tile);
    }

    public void RecreateEdges(Tile tile)
    {
        if (tile == null) return;
        CreateEdges(tile);

        if (tile.X - 1 >= 0)
        {
            CreateEdges(World.Current.Tiles[tile.X - 1, tile.Y]);
        }

        if (tile.X + 1 < 20)
        {
            CreateEdges(World.Current.Tiles[tile.X + 1, tile.Y]);
        }

        if (tile.Y - 1 >= 0)
        {
            CreateEdges(World.Current.Tiles[tile.X, tile.Y-1]);
        }

        if (tile.Y + 1 < 20)
        {
            CreateEdges(World.Current.Tiles[tile.X, tile.Y+1]);
        }
    }

    private void CreateEdges(Tile tile)
    {
        var node = Nodes[tile];
        node.Edges = new List<Edge>();

        if (tile.X - 1 >= 0)
        {
            if (World.Current.Tiles[tile.X - 1, tile.Y].Type == TileType.Enterable)
            {
                node.Edges.Add(new Edge(Nodes[World.Current.Tiles[tile.X - 1, tile.Y]]));
            }
        }

        if (tile.X + 1 < 20)
        {
            if (World.Current.Tiles[tile.X + 1, tile.Y].Type == TileType.Enterable)
            {
                node.Edges.Add(new Edge(Nodes[World.Current.Tiles[tile.X + 1, tile.Y]]));
            }
        }

        if (tile.Y - 1 >= 0)
        {
            if (World.Current.Tiles[tile.X, tile.Y-1].Type == TileType.Enterable)
            {
                node.Edges.Add(new Edge(Nodes[World.Current.Tiles[tile.X, tile.Y-1]]));
            }
        }

        if (tile.Y + 1 < 20)
        {
            if (World.Current.Tiles[tile.X, tile.Y+1].Type == TileType.Enterable)
            {
                node.Edges.Add(new Edge(Nodes[World.Current.Tiles[tile.X, tile.Y+1]]));
            }
        }
    }
}
