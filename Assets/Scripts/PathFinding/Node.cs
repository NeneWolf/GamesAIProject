using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node parent;
    public HexTile target;
    public HexTile destination;
    public HexTile origin;

    public int baseCost;
    public int costFromOrigin;
    public int costToDestination;
    public int pathCost;

    public Node(HexTile current, HexTile origin, HexTile destination, int pathCost)
    {
        parent = null;
        this.target = current;
        this.origin = origin;
        this.destination = destination;

        baseCost = 1;
        costFromOrigin = (int)Vector3.Distance(current.cubeCoordinate, origin.cubeCoordinate);
        costToDestination = (int)Vector3.Distance(current.cubeCoordinate, destination.cubeCoordinate);

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
