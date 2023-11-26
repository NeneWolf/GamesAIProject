using UnityEngine;
using System.Collections;

public class Node
{
    public Node parent;
    public HexTile target;
    public HexTile destination;
    public HexTile origin;

    public int baseCost;
    public int costFromOrigin;
    public int costToDestination;
    public int pathCost;


    public Node (HexTile current, HexTile origin, HexTile destination, int pathCost)
    {
        parent = null;

        this.target = current;
        this.origin = origin;
        this.destination = destination;

        this.baseCost = 1;
        this.costFromOrigin = (int)Vector3.Distance(current.position, origin.position);
        this.costToDestination = (int)Vector3.Distance(current.position, destination.position);
        this.pathCost = pathCost;
    }

    public int GetCost()
    {
        return baseCost + costFromOrigin + costToDestination + pathCost;
    }

    public void SetParent(Node node)
    {
        this.parent = node; 
    }


}
