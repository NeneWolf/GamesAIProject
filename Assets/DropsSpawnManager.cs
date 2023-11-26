using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsSpawnManager : MonoBehaviour
{
    GameManager gameManager;
    TileManager tileManager;

    [SerializeField] float initialDropSpawningTimer;
    [SerializeField] float timerBetweenActiveCollectedDrops;
    float time;
    [SerializeField] GameObject[] drops;

    int maxDrops;
    int activeDrops;

    List<HexTile> allTiles;
    public List<HexTile> freeTiles;

    // Start is called before the first frame update
    void Awake()
    {
        tileManager = GameObject.FindAnyObjectByType<TileManager>();
        gameManager = GameObject.FindAnyObjectByType<GameManager>();

        maxDrops = 5;
        time = initialDropSpawningTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.hasGameStarted)
        {
            if(activeDrops != maxDrops)
                time -= Time.deltaTime;

            if (time <= 0f)
            {
                GrabAllTiles();
                freeTiles.Clear();
                time = timerBetweenActiveCollectedDrops;
            }
        }
    }

    void SpawnDrops()
    {
        while(activeDrops != maxDrops)
        {
            int randomTile = Random.Range(0, freeTiles.Count);
            int randomDrop = Random.Range(0, drops.Length);

            Instantiate(drops[randomDrop], freeTiles[randomTile].transform.position + new Vector3(0, 20f, 0), Quaternion.identity);
            freeTiles.Remove(freeTiles[randomTile]);
            activeDrops++;
        }
    }

    void GrabAllTiles()
    {
        allTiles = tileManager.allHexigons;

        foreach (HexTile tile in allTiles)
        {
            if (!tile.hasObjects && tile.heightWeight <= 0.7f)
            {
                freeTiles.Add(tile);
            }
        }

        SpawnDrops();
    }

    public void DropDestroyed()
    {
        activeDrops--;
    }
   
}
