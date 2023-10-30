using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding
{
    private HexagonGrid<PathNode> grid;

    public EnemyPathFinding(int width, int height)
    {
       grid = new HexagonGrid<PathNode>(width, height,10f,Vector3.zero, (HexagonGrid<PathNode> g, int x, int y) => new PathNode(g, x, y),null);

    }
}
