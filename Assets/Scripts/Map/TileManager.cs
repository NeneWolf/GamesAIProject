using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] GameObject hexHighLightTile;
    [SerializeField] GameObject hexSelectorTile;

    private GameObject highTile;
    private GameObject selectorTile;

    Dictionary<Vector3Int, HexTile> tiles;
    //Testing
    public List<HexTile> allHexigons;

    public Transform HighTiles;
    public Transform SelectorTiles;

    public TileManager instance;

    public Vector3Int playerPos { get; internal set; }

    private void Awake()
    {
        instance = this;

        highTile = Instantiate(hexHighLightTile, new Vector3(0, 20, 0), Quaternion.identity);
        selectorTile = Instantiate(hexSelectorTile, new Vector3(0, 20, 0), Quaternion.identity);
    }


    public  void Assign()
    {
        tiles = new Dictionary<Vector3Int, HexTile>();

        //Check if there is any HexParents in the scene
        GameObject[] hexParent = GameObject.FindGameObjectsWithTag("MapTiles");

        foreach (GameObject hexGroup in hexParent)
        {
            HexTile[] hexTiles = hexGroup.GetComponentsInChildren<HexTile>();

            foreach (HexTile tile in hexTiles)
            {
                RegisterTile(tile);
            }

            foreach (HexTile tile in hexTiles)
            {
                List<HexTile> neighbors = GetNeightbour(tile);
                tile.neighbours = neighbors;
            }
        }

        //Testing
        allHexigons = tiles.Values.ToList();
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



    public void OnHighlightTile(HexTile tile)
    {
        RaycastHit hit;

        if (Physics.Raycast(tile.transform.position + new Vector3(0,50,0), Vector3.down, out hit, 100f) && hit.collider.gameObject.layer == 6)
        {
            highTile.transform.position = new Vector3(tile.position.x, hit.point.y, tile.position.z);
        }
    }

    public void OnSelectTile(HexTile tile)
    {
        RaycastHit hit;

        if (Physics.Raycast(tile.transform.position + new Vector3(0, 50, 0), Vector3.down, out hit, 100f) && hit.collider.gameObject.layer == 6)
        {
            selectorTile.transform.position = new Vector3(tile.position.x, hit.point.y, tile.position.z);
        }
    }

    public HexTile GetRandomTile()
    {
        
        return tiles.ElementAt(Random.Range(0, tiles.Count)).Value;
    }

}





