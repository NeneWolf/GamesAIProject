using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap,FallOffMap,HexMap };

    public DrawMode drawMode;

    [Header("Map Settings")]
    [SerializeField]private HexOrientation hexOrientation;
    [SerializeField]private int mapWidth;
    [SerializeField]private int mapHeight;
    [SerializeField]private float hexSize;

    //Noise Settings
    [Header("Noise Settings")]
    [SerializeField]private float noiseScale = 0.3f;
    //[SerializeField] private float noiseFrequency = 100f;

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
        if (this.gameObject.transform.childCount != 0)
            DeleteAllchildren();

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

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.FallOffMap)
        { display.DrawTexture(TextureGenerator.TextureFromHeightMap(FallOffGenerator.GenerateFallOffMap(mapWidth, mapHeight))); }
        else if (drawMode == DrawMode.HexMap) { HexMapGenerator(); }

    }
    void HexMapGenerator()
    {
        //Generate Hexagon Tiles
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                if (useFallOff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallOffMap[x, y]);
                }

                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height && currentHeight > 0.2f) // Change this condition depending on the tile near the "lava"
                    {
                        GameObject hexagonTile = Instantiate(regions[i].tilePrefab, HexMetrics.Center(hexSize, x, y, hexOrientation) + transform.position, Quaternion.identity);
                        hexagonTile.transform.parent = transform;
                        break;
                    }
                }
            }
        }
    }
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

    private void DeleteAllchildren()
    {
        int childCount = this.gameObject.transform.childCount;
        print("I am suppose to delete but i am not");
        for (int i = 0; i < childCount; i++)
        {
            DestroyImmediate(this.gameObject.transform.GetChild(0).gameObject);
        }
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

    }
}

public enum HexOrientation
{
    FlatTop,
    PointyTop
}



