using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private HexagonGrid<PathNode> grid;

    int x;
    int y;
    int gCost;
    int hCost;
    int fCost;

    public PathNode cameFromNode;


    public PathNode(HexagonGrid<PathNode>grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
