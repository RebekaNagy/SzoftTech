using System;
using System.Collections.Generic;
using System.Linq;

//Útkereseő algoritmus, A* alapján, heurisztikája a Manhattan-távolság
public class PathFinder
{
    private static World world = World.Current;

    public static Queue<Tile> FindPath(Tile starTile, Tile destinationTile, Tile noEntry = null)
    {
        if (world != World.Current)
        {
            world = World.Current;
        }

        if (starTile == null || destinationTile == null || (!world?.Graph?.Nodes.ContainsKey(starTile) ?? true)) return null;

        var nodes = world.Graph.Nodes;

        var start = nodes[starTile];

        var closedSet = new HashSet<Node>();

        var openSet = new PriorityQueue();
        openSet.Enqueue(start, 0);

        var cameFrom = new Dictionary<Node, Node>();

        var gScore = new Dictionary<Node, float> { [start] = 0 };

        var fScore = new Dictionary<Node, float> { [start] = ManhattanDistance(start.Tile, destinationTile) };

        while (openSet.Count() > 0)
        {
            var current = openSet.Dequeue();

            if (destinationTile == current.Tile) return Reconstruct(cameFrom, current);

            closedSet.Add(current);

            foreach (var edgeNeighbor in current.Edges)
            {
                var neighbor = edgeNeighbor.Node;

                if (noEntry != null && neighbor.Tile == noEntry) continue;

                if (closedSet.Contains(neighbor)) continue;

                var tentativeGScore = gScore[current] + edgeNeighbor.MovementCost;

                if (openSet.Contains(neighbor) && tentativeGScore >= gScore[neighbor]) continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + ManhattanDistance(neighbor.Tile, destinationTile);

                openSet.Enqueue(neighbor, fScore[neighbor]);
            }
        }

        return null;
    }

    private static float ManhattanDistance(Tile a, Tile b)
    {
        return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y));
    }

    //Visszaadja az utat
    private static Queue<Tile> Reconstruct(IReadOnlyDictionary<Node, Node> cameFrom, Node current)
    {
        var path = new Queue<Tile>();
        path.Enqueue(current.Tile);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Enqueue(current.Tile);
        }

        return new Queue<Tile>(path.Reverse());
    }
}
