using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.AI;


public class MapGenerator : MonoBehaviour
{
    public EnemyPathFinding enemyPathFindingGrid;

    public enum DrawMode { NoiseMap,FallOffMap,HexMap };

    public DrawMode drawMode;

    [SerializeField]private GameObject HexParent;
    private GameObject emptyParent;

    [Header("Map Settings")]
    [SerializeField]private HexOrientation hexOrientation;
    [SerializeField]private int mapWidth;
    [SerializeField]private int mapHeight;
    [SerializeField]private float hexSize;

    //Noise Settings
    [Header("Noise Settings")]
    [SerializeField]private float noiseScale = 0.3f;
    //[SerializeField] private float noiseFrequency = 100f;

    float currentHeight;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int octaves;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    float[,] noiseMap;
    float[,] fallOffMap;

    public bool autoUpdate;
    public TileTypes[] regions;

    public bool useFallOff;
    public bool spawnDecor;
    bool hasCleaned = true;

    GameObject hexagonTile;


    //Map Decorations
    [Header("Map Decorations")]

    GameObject detail;
    [SerializeField] GameObject[] KeyBuildings;
    [SerializeField]int numberOfVillages;
    public List<GameObject> playerArea;
    public List<GameObject> enemyArea;

    bool hasSpawnVillage = false;
    bool hasSpawnCastle = false;
    bool hasSpawnEnemyCastle = false;

    [Header("NavMesh")]
    GameObject surface;

    [SerializeField] private GameObject hexagonGridPref;
    [SerializeField] private GameObject hexagonGridViewPrefab;

    private void Awake()
    {
        fallOffMap = FallOffGenerator.GenerateFallOffMap(mapWidth, mapHeight);
    }

    private void GeneratePerlinNoise()
    {
        noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
    }

    public void GenerateMap()
    {
        GeneratePerlinNoise();
        MapDisplay display = FindAnyObjectByType<MapDisplay>();

        //Generate Hexagon Tiles
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x,y] - fallOffMap[x,y]);
                }
            }
        }

        // ADD TO TURN ON THE PLANE TO VIEW THE NOISE MAP & FALL OFF MAP
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.FallOffMap)
        { display.DrawTexture(TextureGenerator.TextureFromHeightMap(FallOffGenerator.GenerateFallOffMap(mapWidth, mapHeight))); }
        else if (drawMode == DrawMode.HexMap) { HexMapGenerator(); }

    }

    void HexMapGenerator()
    {
        //clean the list
        if(playerArea.Count > 0) playerArea.Clear();
        if(enemyArea.Count > 0) enemyArea.Clear();

        //Check if there is any HexParents in the scene
        GameObject[] var = GameObject.FindGameObjectsWithTag("MapTiles");

        //Destroy all if found more then 1
        if (HexParent != null || var.Length > 1)
        {
            foreach (GameObject obj in var)
            {
                DestroyImmediate(obj);
            }
        }

        //Create the "HexParent" Object
        HexParent = new GameObject("HexParentTiles");
        HexParent.AddComponent<Unity.AI.Navigation.NavMeshSurface>();
        HexParent.transform.position = Vector3.zero;
        HexParent.transform.tag = "MapTiles";

        //Generate Hexagon Tiles
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                if (useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallOffMap[x, y]);
                }

                currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height && currentHeight > 0.10f) // Change this condition depending on the tile near the "lava"
                    {
                        //Create Empty Parent for HexTile
                        emptyParent = new GameObject("HexTileParent");
                        emptyParent.transform.parent = HexParent.transform;

                        //Spawn the Hex Tiles
                        hexagonTile = Instantiate(regions[i].tilePrefab, HexMetrics.Center(hexSize, x, y, hexOrientation) + transform.position, Quaternion.identity);

                        //Change the information
                        hexagonTile.name = x + "," + y;
                        hexagonTile.layer = 6;
    
                        hexagonTile.transform.localScale = new Vector3(5, meshHeightCurve.Evaluate(noiseMap[x, y]) * meshHeightMultiplier, 5);
                        hexagonTile.transform.parent = emptyParent.transform;

                        //Add the HexTile script & information
                        hexagonTile.AddComponent<HexTile>();
                        hexagonTile.GetComponent<HexTile>().collisionMesh = hexagonTile.GetComponent<MeshFilter>().mesh.bounds.extents;
                        hexagonTile.GetComponent<HexTile>().scale = hexagonTile.transform.lossyScale;
                        hexagonTile.GetComponent<HexTile>().position = hexagonTile.transform.position;
                        hexagonTile.GetComponent<HexTile>().offSetCoordinate = new Vector2Int(x, y);
                        hexagonTile.GetComponent<HexTile>().cubeCoordinate = OffSetToCube(hexagonTile.GetComponent<HexTile>().offSetCoordinate);



                        AddTilesIntoSection(i);


                        //Remove coment to add the decor
                        //// Spawn the object on top of the hexagon
                        if (spawnDecor)
                        {
                            SpawnMapDecoration(hexagonTile, i);
                        }
                    
                        break;
                    }
                }
            }
        }

        //Spawn Key Buildings
        if (spawnDecor)
        {
            if (numberOfVillages != 0 && hasSpawnVillage == false)
            {
                for (int z = 0; z < numberOfVillages; z++)
                {
                    int randomTile = Random.Range(0, playerArea.Count);
                    hexagonTile = playerArea[randomTile];
                    detail = Instantiate(KeyBuildings[0], playerArea[randomTile].gameObject.transform.position, playerArea[randomTile].transform.transform.rotation, playerArea[randomTile].gameObject.transform.parent);

                    detail.GetComponent<DetailMovement>().UpdatePosition();
                    playerArea.RemoveAt(randomTile);
                }
            }

            if (hasSpawnCastle == false)
            {
                int randomTile = Random.Range(0, playerArea.Count);
                hexagonTile = playerArea[randomTile];
                detail = Instantiate(KeyBuildings[1], playerArea[randomTile].gameObject.transform.position, playerArea[randomTile].transform.transform.rotation, playerArea[randomTile].gameObject.transform.parent);

                detail.GetComponent<DetailMovement>().UpdatePosition();
                playerArea.Clear();
            }

            if (hasSpawnEnemyCastle == false)
            {
                int randomTile = Random.Range(0, enemyArea.Count);
                hexagonTile = enemyArea[randomTile];
                detail = Instantiate(KeyBuildings[2], enemyArea[randomTile].transform.position, enemyArea[randomTile].transform.transform.rotation, enemyArea[randomTile].gameObject.transform.parent);
                detail.GetComponent<DetailMovement>().UpdatePosition();
                enemyArea.Clear();
            }
        }

        //Generate Grid
        GetComponent<TileManager>().Assign();

        //Generate navmesh
        GenerateNavMesh();
    }

    void SpawnMapDecoration(GameObject hexagonTile, int i)
    {
        float randomIndex = Random.Range(0f, 1f);

        if (regions[i].detailPrefabs.Length > 0)
        {
            //Spawn the decoration
            if(randomIndex > 0.5)
            {
                if (regions[i].detailPrefabs.Length > 1)
                {
                    int random = Random.Range(0, regions[i].detailPrefabs.Length);

                    detail = Instantiate(regions[i].detailPrefabs[random].detailPrefab, hexagonTile.transform.position, hexagonTile.transform.transform.rotation, emptyParent.transform);
                    detail.GetComponent<DetailMovement>().UpdatePosition();

                    if (playerArea.Contains(hexagonTile.gameObject)) { playerArea.Remove(hexagonTile.gameObject); }
                    else if (enemyArea.Contains(hexagonTile.gameObject)) { enemyArea.Remove(hexagonTile.gameObject); }
                }
                else
                {
                    float random = Random.Range(0, 100);

                    if (random < regions[i].detailPrefabs[0].spawnChange)
                    {
                        detail = Instantiate(regions[i].detailPrefabs[0].detailPrefab, hexagonTile.transform.position, hexagonTile.transform.transform.rotation, emptyParent.transform);
                        detail.GetComponent<DetailMovement>().UpdatePosition();

                        if (playerArea.Contains(hexagonTile.gameObject)) { playerArea.Remove(hexagonTile.gameObject);} 
                        else if(enemyArea.Contains(hexagonTile.gameObject)) { enemyArea.Remove(hexagonTile.gameObject); }
                    }
                }
            }
        }
    }

    void AddTilesIntoSection(int i)
    {
        //Spawn the villages
        if (regions[i].height <= 0.6f)
        {
            // add tile into the list of playerArea
            playerArea.Add(hexagonTile.gameObject);
        }
        else if (regions[i].height <= 1f && regions[i].height > 0.75f)
        {
            // add tile into the list of enemyArea
            enemyArea.Add(hexagonTile.gameObject);
        }
    }

    //Return the parent that has just spawned
    public GameObject ReturnTileParent()
    {
        return hexagonTile;
    }


    private void OnValidate()
    {
        if (mapWidth < 1) { mapWidth = 1; }
        if (mapHeight < 1) { mapHeight = 1; }
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 0; }

        fallOffMap = FallOffGenerator.GenerateFallOffMap(mapWidth, mapHeight);
    }

    //Types of areas on the map using height as a measurement
    [System.Serializable]
    public struct TileTypes
    {
        public string name;
        public float height;
        public GameObject tilePrefab;
        public TileDecor[] detailPrefabs;
    }

    [System.Serializable]
    public struct TileDecor
    {
        public GameObject detailPrefab;
        public float spawnChange;
    }

    //Generate Mesh at runTime // NOT THE BEST TO BE PUBLIC BUT IT IS WHAT IT IS...
    public void GenerateNavMesh()
    {
        HexParent.GetComponent<Unity.AI.Navigation.NavMeshSurface>().BuildNavMesh();
    }

    //void CreateNewGrid()
    //{
    //    enemyPathFindingGrid = new EnemyPathFinding(mapWidth,mapHeight,(hexSize * 2) - 0.8f,hexagonGridPref);

    //    for(int x = 0; x < mapWidth; x++)
    //    {
    //        for(int z = 0; z < mapHeight; z++)
    //        {
    //            Instantiate(hexagonGridViewPrefab, new Vector3(), Quaternion.identity);
    //        }
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    for (int x = 0; x < mapHeight; x++)
    //    {
    //        for (int z = 0; z < mapWidth; z++)
    //        {

    //            Vector3 centerPosition = HexMetrics.Center(hexSize, x, z, hexOrientation) + transform.position;
    //            for (int s = 0; s < HexMetrics.GetCorners(hexSize, hexOrientation).Length; s++)
    //            {

    //                Vector3 start = HexMetrics.GetCorners(hexSize, hexOrientation)[s] + centerPosition;
    //                Vector3 end = HexMetrics.GetCorners(hexSize, hexOrientation)[(s + 1) % HexMetrics.GetCorners(hexSize, hexOrientation).Length] + centerPosition;
    //                Gizmos.DrawLine(start, end);
    //            }
    //        }
    //    }
    //}


    public static Vector3Int OffSetToCube(Vector2Int offset)
    {
        var q = offset.x - (offset.y - (offset.y % 2)) / 2;
        var r = offset.y;
        return new Vector3Int(q,r,-q-r);
    }

    
}

public enum HexOrientation
{
    FlatTop,
    PointyTop
}




