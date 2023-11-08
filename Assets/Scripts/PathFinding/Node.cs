using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node parent;
    public GameObject target;
    public GameObject destination;
    public GameObject origin;

    public int baseCost;
    public int costFromOrigin;
    public int costToDestination;
    public int pathCost;

    public Node point;

    public Node (GameObject current, GameObject origin, GameObject destination, int pathCost)
    {
        parent = null;
        this.baseCost = 1;
        this.costFromOrigin = (int)Vector3.Distance(current.GetComponent<HexTile>().position, origin.GetComponent<HexTile>().position);
        this.costToDestination = (int)Vector3.Distance(current.GetComponent<HexTile>().position, destination.GetComponent<HexTile>().position);
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
