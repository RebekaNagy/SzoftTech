using System;

//Gráf éleinek osztálya
public class Edge
{
    public float MovementCost; //út költsége
    public Node Node; //Amelyik csúcsba megy


    public Edge(Node node)
    {
        Node = node;
        MovementCost = 1;
    }
}
