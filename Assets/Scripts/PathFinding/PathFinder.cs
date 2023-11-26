using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    static Dictionary<HexTile, Node> analised = new Dictionary<HexTile, Node>();
    static Dictionary<HexTile, Node> notanalised = new Dictionary<HexTile, Node>();

    static bool DecorCheck;

    public static List<HexTile> FindPath(HexTile origin, HexTile destination, bool isForDecorCheck)
    {
        Dictionary<HexTile, Node> nodesNotEvaluated = new Dictionary<HexTile, Node>();
        Dictionary<HexTile, Node> nodesAlreadyEvaluated = new Dictionary<HexTile, Node>();

        DecorCheck = isForDecorCheck;

        Node startNode = new Node(origin, origin, destination, 0);

        nodesNotEvaluated.Add(origin, startNode);

        bool gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out List<HexTile> Path);

        while (!gotPath)
        {
            gotPath = EvaluateNextNode(nodesNotEvaluated, nodesAlreadyEvaluated, origin, destination, out Path);
        }
        analised = nodesNotEvaluated;
        notanalised = nodesAlreadyEvaluated;

        Path.Reverse();
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

        nodesNotEvaluated.Remove(currentNode.target);
        nodesAlreadyEvaluated.Add(currentNode.target, currentNode);

        Path = new List<HexTile>();

        //if we have reached the destination, we are done return true
        if(currentNode.target == destination)
        {
            Path.Add(currentNode.target);
            while (currentNode.target != origin)
            {
                Path.Add(currentNode.parent.target);
                currentNode = currentNode.parent;
            }

            return true;
        }

        // otherwise, we need to evaluate the neighbours of the current node 
        List<Node> neightbours = new List<Node>();

        foreach(HexTile tile in currentNode.target.neighbours)
        {
            Node node = new Node(tile, origin, destination, currentNode.GetCost());

            if (!DecorCheck)
            {
                //if the node isn't something we can reverse
                if (tile.hasObjects)
                {
                    node.baseCost = 9999;
                    //continue;
                }
                else if (tile.heightWeight >= 0.768f && tile.heightWeight <= 0.9f) // Avoid "higher" areas
                {
                    node.baseCost = 500;
                }
                else if (tile.heightWeight >= 0.2f && tile.heightWeight <= 0.65f)
                {
                    node.baseCost = 0;
                }

            }

            neightbours.Add(node);
        }

        foreach(Node neighbour in neightbours)
        {
            // if the tile has been already evaluated flly we can ignore it
            if (nodesAlreadyEvaluated.Keys.Contains(neighbour.target))
            {
                continue;
            }

            // if the cost is lower or if the tile isnt in the evaluated pile
            if(neighbour.GetCost() < currentNode.GetCost() ||
                !nodesNotEvaluated.Keys.Contains(neighbour.target))
            {
                neighbour.SetParent(currentNode);
                if(!nodesNotEvaluated.Keys.Contains(neighbour.target))
                {
                    nodesNotEvaluated.Add(neighbour.target, neighbour);
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

    public List<HexTile> GetTileListAnalised()
    {
        List<HexTile> a = new List<HexTile>();
        foreach (HexTile tile in analised.Keys)
        {
            a.Add(tile);
        }

        return a;
    }


    public List<HexTile> GetTileListNotAnalised()
    {
        List<HexTile> b = new List<HexTile>();
        foreach (HexTile tile in notanalised.Keys)
        {
            b.Add(tile);
        }
        
        return b;
    }
}
