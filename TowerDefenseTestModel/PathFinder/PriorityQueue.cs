using System;
using System.Collections.Generic;
using System.Linq;

//Prioritásos sor osztálya, MinPriorQueue
public class PriorityQueue
{
    public Dictionary<Node, float> MinPriorityQueue;

    public PriorityQueue()
	{
        MinPriorityQueue = new Dictionary<Node, float>();
    }

    public bool Contains(Node data)
    {
        return MinPriorityQueue?.ContainsKey(data) ?? false;
    }

    public Node Dequeue()
    {
        Node minNode = MinPriorityQueue.ElementAt(0).Key;
        float minPriority = MinPriorityQueue.ElementAt(0).Value;

        foreach(var item in MinPriorityQueue)
        {
            if(item.Value < minPriority)
            {
                minNode = item.Key;
                minPriority = item.Value;
            }
        }

        MinPriorityQueue.Remove(minNode);

        return minNode;
    }

    public void Enqueue(Node data, float priority)
    {
        if (MinPriorityQueue.ContainsKey(data))
        {
            MinPriorityQueue[data] = priority;
        }
        else
        {
            MinPriorityQueue.Add(data, priority);
        }
    }

    public int Count()
    {
        return MinPriorityQueue.Count;
    }
}
