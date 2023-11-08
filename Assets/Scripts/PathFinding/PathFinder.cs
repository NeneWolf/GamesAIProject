using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static List<HexTile> FindPath(HexTile origin, HexTile destination)
    {
        Dictionary<HexTile, Node> nodesNotEvaluated = new Dictionary<HexTile, Node>();
        Dictionary<HexTile, Node> nodesAlreadyEvaluated = new Dictionary<HexTile, Node>();

        Node startNode = new Node(origin.GetComponent<HexTile>().gameObject, origin.GetComponent<HexTile>().gameObject, destination.GetComponent<HexTile>().gameObject, 0);

        nodesNotEvaluated.Add(origin, startNode);

        bool gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out List<HexTile> Path);

        if (startNode == null)
        {
            Debug.LogError("startNode is null. Check the Node constructor.");
            return null;
        }

        while (!gotPath)
        {
            EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out Path);
        }

        return Path;

    }

    public static bool EvaluateNextNode(Dictionary<HexTile, Node> nodesNotEvaluated,
        Dictionary<HexTile, Node> nodesAlreadyEvaluated,
        HexTile origin,
        HexTile destination,
        out List<HexTile> Path)
    {
        Node currentNode = GetCheapestNode(nodesNotEvaluated.Values.ToArray());

        if(currentNode == null)
        {
            Path = new List<HexTile>();
            return false;
        }

        nodesNotEvaluated.Remove(currentNode.target.GetComponent<HexTile>());
        nodesAlreadyEvaluated.Add(currentNode.target.GetComponent<HexTile>(), currentNode);

        Path = new List<HexTile>();

        //if we have reached the destination, we are done return true
        if(currentNode.target == destination)
        {
            Path.Add(currentNode.target.GetComponent<HexTile>());
            while (currentNode.target != origin)
            {
                Path.Add(currentNode.parent.target.GetComponent<HexTile>());
                currentNode = currentNode.parent;
            }

            return true;
        }

        // otherwise, we need to evaluate the neighbours of the current node 
        List<Node> neightbours = new List<Node>();

        foreach(HexTile tile in currentNode.target.GetComponent<HexTile>().neighbours)
        {
            Node node = new Node(tile.GetComponent<HexTile>().gameObject, origin.GetComponent<HexTile>().gameObject, destination.GetComponent<HexTile>().gameObject, currentNode.GetCost());

            //if the node isn't something we can reverse
            if (tile.hasObjects)
            {
                //node.baseCost = 9999;
                continue;
            }

            neightbours.Add(node);
        }

        foreach(Node neighbour in neightbours)
        {
            // if the tile has been already evaluated flly we can ignore it
            if (nodesAlreadyEvaluated.Keys.Contains(neighbour.target.GetComponent<HexTile>()))
            {
                continue;
            }

            // if the cost is lower or if the tile isnt in the evaluated pile
            if(neighbour.GetCost() < currentNode.GetCost() ||
                !nodesNotEvaluated.Keys.Contains(neighbour.target.GetComponent<HexTile>()))
            {
                neighbour.SetParent(currentNode);
                if(!nodesNotEvaluated.Keys.Contains(neighbour.target.GetComponent<HexTile>()))
                {
                    nodesNotEvaluated.Add(neighbour.target.GetComponent<HexTile>(), neighbour);
                }
            }
        }
        
        return false;
    }

    // Find the cheapest node in the list
    public static Node GetCheapestNode(Node[] nodesNotEvaluated)
    {
        //If it has no neighbours, return null
        if (nodesNotEvaluated.Length == 0) { return null; }

        //if not , go over the rest
        Node selectedNode = nodesNotEvaluated[0];

        for (int i = 1; i < nodesNotEvaluated.Length; i++)
        {
            var currentNode = nodesNotEvaluated[i];

            if (currentNode.GetCost() < selectedNode.GetCost())
            {
                selectedNode = currentNode;
            }
            else if (currentNode.GetCost() == selectedNode.GetCost() &&
                currentNode.costToDestination < selectedNode.costToDestination)
            {
                selectedNode = currentNode;
            }
        }
        return selectedNode;
    }
}
