using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node parent;
    public HexTile target = new HexTile();
    //public HexTile destination;
    //public HexTile origin;

    public int baseCost;
    public int costFromOrigin;
    public int costToDestination;
    public int pathCost;

    public Node(HexTile current, HexTile origin, HexTile destination, int pathCost)
    {
        parent = null;
        target = current;

        baseCost = 1;
        costFromOrigin = (int)Vector3.Distance(current.cubeCoordinate, origin.cubeCoordinate);
        costToDestination = (int)Vector3.Distance(current.cubeCoordinate, destination.cubeCoordinate);


        Debug.Log(current.transform.name);
        Debug.Log("Origin: " + origin.transform.name);
        Debug.Log("Destination: " + destination.transform.name);
        Debug.Log("Base cost: " + baseCost);
        Debug.Log("Cost from origin: " + costFromOrigin);
        Debug.Log("Cost to destination: " + costToDestination);
        Debug.Log("Path cost: " + pathCost);

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
