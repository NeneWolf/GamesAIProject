using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyPathFinding
{
    //private const int moveStraightCost = 10;
    //private const int moveDiagonalCost = 14;

    //private HexagonGrid<PathNode> grid;
    //private List<PathNode> openList;
    //private List<PathNode> closedList;

    //public EnemyPathFinding(int width, int height, float size, GameObject parent)
    //{
    //   grid = new HexagonGrid<PathNode>(width, height, size, Vector3.zero, (HexagonGrid<PathNode> g, int x, int y) => new PathNode(g, x, y), parent);

    //}

    //public List<PathNode> Findpath(int startX, int startZ, int endX, int endZ)
    //{
    //    PathNode startNode = grid.GetGridObject(startX, startZ);
    //    PathNode endNode = grid.GetGridObject(endX, endZ);

    //    openList = new List<PathNode> { startNode };
    //    closedList = new List<PathNode>();

    //    for(int x = 0; x < grid.width; x++)
    //    {
    //        for(int z = 0; z < grid.height; z++)
    //        {
    //            PathNode pathNode = grid.GetGridObject(x, z);
    //            pathNode.gCost = int.MaxValue;
    //            pathNode.CalculateFCost();
    //            pathNode.cameFromNode = null;
    //        }
    //    }

    //    startNode.gCost = 0;
    //    startNode.hCost = CalculateDistanceCost(startNode, endNode);
    //    startNode.CalculateFCost();

    //    while(openList.Count > 0)
    //    {
    //        PathNode currentNode = GetLowestFCostNode(openList);

    //        if(currentNode == endNode)
    //        {
    //            return CalculatePath(endNode);
    //        }

    //        openList.Remove(currentNode);
    //        closedList.Add(currentNode);

    //        foreach(PathNode neighbours in GetNeighbourList(currentNode))
    //        {
    //            if(closedList.Contains(neighbours))
    //            {
    //                continue;
    //            }
                
    //            if(!neighbours.walkable)
    //            {
    //                closedList.Add(neighbours);
    //                continue;
    //            }

    //            int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbours);

    //            if(tentativeGCost < neighbours.gCost)
    //            {
    //                neighbours.cameFromNode = currentNode;
    //                neighbours.gCost = tentativeGCost;
    //                neighbours.hCost = CalculateDistanceCost(neighbours, endNode);
    //                neighbours.CalculateFCost();

    //                if(!openList.Contains(neighbours))
    //                {
    //                    openList.Add(neighbours);
    //                }
    //            }
    //        }
    //    }

    //    return null;
    //}

    //public PathNode GetNode(int x, int z)
    //{
    //    return grid.GetGridObject(x, z);
    //}


    //private List<PathNode> GetNeighbourList(PathNode currentNode)
    //{
    //    List<PathNode> neighbours = new List<PathNode>();

    //    #region
    //    //for (int x = -1; x <= 1; x++)
    //    //{
    //    //    for (int y = -1; y <= 1; y++)
    //    //    {
    //    //        if (x == 0 && y == 0)
    //    //            continue;

    //    //        int checkX = currentNode.x + x;
    //    //        int checkY = currentNode.z + y;

    //    //        if (checkX >= 0 && checkX < grid.width && checkY >= 0 && checkY < grid.he)
    //    //        {
    //    //            neighbours.Add(grid.SetGridObject[checkX, checkY]);
    //    //        }
    //    //    }
    //    //}

    //    //return neighbours;
    //    #endregion

    //    if(currentNode.x -1 >= 0)
    //    {
    //        neighbours.Add(GetNode(currentNode.x - 1, currentNode.z));

    //        if(currentNode.z - 1 >= 0)
    //        {
    //            neighbours.Add(GetNode(currentNode.x - 1, currentNode.z - 1));
    //        }

    //        if(currentNode.z + 1 < grid.height)
    //        {
    //            neighbours.Add(GetNode(currentNode.x - 1, currentNode.z + 1));
    //        }
    //    }

    //    if(currentNode.x + 1 < grid.width)
    //    {
    //        neighbours.Add(GetNode(currentNode.x + 1, currentNode.z));

    //        if(currentNode.z - 1 >= 0)
    //        {
    //            neighbours.Add(GetNode(currentNode.x + 1, currentNode.z - 1));
    //        }

    //        if(currentNode.z + 1 < grid.height)
    //        {
    //            neighbours.Add(GetNode(currentNode.x + 1, currentNode.z + 1));
    //        }
    //    }

    //    if(currentNode.z - 1 >= 0)
    //    {
    //        neighbours.Add(GetNode(currentNode.x, currentNode.z - 1));
    //    }

    //    if(currentNode.z + 1 < grid.height)
    //    {
    //        neighbours.Add(GetNode(currentNode.x, currentNode.z + 1));
    //    }

    //    return neighbours;
    //}


    //private List<PathNode> CalculatePath (PathNode endNode)
    //{
    //    List<PathNode> path = new List<PathNode>();
    //    path.Add(endNode);
    //    PathNode currentNode = endNode;
    //    while (currentNode != null)
    //    {
    //        path.Add(currentNode);
    //        currentNode = currentNode.cameFromNode;
    //    }

    //    path.Reverse();
    //    return path;
    //}

    //public HexagonGrid<PathNode> GetGrid()
    //{
    //    return grid;
    //}

    //private int CalculateDistanceCost(PathNode a, PathNode b)
    //{
    //    int xDistance = Mathf.Abs(a.x - b.x);
    //    int zDistance = Mathf.Abs(a.z - b.z);
    //    int remaining = Mathf.Abs(xDistance - zDistance);
    //    return moveDiagonalCost * Mathf.Min(xDistance, zDistance) + moveStraightCost * remaining;
    //}

    //private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    //{
    //    PathNode lowestFCostNode = pathNodeList[0];
    //    for (int i = 1; i < pathNodeList.Count; i++)
    //    {
    //        if (pathNodeList[i].fCost < lowestFCostNode.fCost)
    //        {
    //            lowestFCostNode = pathNodeList[i];
    //        }
    //    }
    //    return lowestFCostNode;
    //}
}
