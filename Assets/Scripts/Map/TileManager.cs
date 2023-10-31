using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] GameObject hexHighLightTile;
    [SerializeField] GameObject hexSelectorTile;
    private GameObject highTile;
    private GameObject selectorTile;
    Dictionary<Vector3Int, HexTile> tiles;

    public Transform HighTiles;
    public Transform SelectorTiles;

    public  void Assign()
    {
        tiles = new Dictionary<Vector3Int, HexTile>();

        //Check if there is any HexParents in the scene
        GameObject[] hexParent = GameObject.FindGameObjectsWithTag("MapTiles");
        
        foreach(GameObject hexGroup in hexParent)
        {
            HexTile[] hexTiles = hexGroup.GetComponentsInChildren<HexTile>();

            foreach(HexTile tile in hexTiles)
            {
                RegisterTile(tile);
            }

            foreach(HexTile tile in hexTiles)
            {
                List<HexTile> neighbors = GetNeightbour(tile);
                tile.neighbors = neighbors;
            }
        }
    }

    public void RegisterTile(HexTile tile)
    {
        tiles.Add(tile.cubeCoordinate, tile);
    }

    private List<HexTile> GetNeightbour(HexTile tile)
    {
        List<HexTile> neighbors = new List<HexTile>();

        Vector3Int[] neightbourCoords = new Vector3Int[]
        {
            new Vector3Int(1, -1, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 1, -1),
            new Vector3Int(-1, 1, 0),
            new Vector3Int (-1, 0, 1),
            new Vector3Int(0,-1,1 ),
        };

        foreach(Vector3Int neighourCoord in neightbourCoords)
        {
            Vector3Int tileCoord = tile.cubeCoordinate;

            if(tiles.TryGetValue(tileCoord + neighourCoord, out HexTile neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private void Awake()
    {
        highTile = Instantiate(hexHighLightTile, Vector3.zero, Quaternion.identity);
        selectorTile = Instantiate(hexSelectorTile, Vector3.zero, Quaternion.identity);
    }

    public void OnHighlightTile(HexTile tile)
    {
        Vector3 parentPosition = tile.GetComponent<MeshFilter>().mesh.bounds.extents;
        Vector3 currentGlobalScale = tile.scale;

        highTile.transform.position = tile.position + new Vector3(0, parentPosition.y * currentGlobalScale.y + 0.5f, 0);
        HighTiles = highTile.transform;
    }

    public void OnSelectTile(HexTile tile)
    {
        Vector3 parentPosition = tile.GetComponent<MeshFilter>().mesh.bounds.extents;
        Vector3 currentGlobalScale = tile.scale;

        selectorTile.transform.position = tile.position + new Vector3(0, parentPosition.y * currentGlobalScale.y + 0.5f, 0);
        SelectorTiles = selectorTile.transform;
    }

}
