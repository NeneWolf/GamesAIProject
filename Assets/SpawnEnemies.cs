using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    GameManager gameManager;
    DetailMovement detailMovement;
    [SerializeField]GameObject[] enemies;

    [SerializeField] float initialSpawningTimer;
    [SerializeField] float timerBetweenEnemies;

    float time;
    int maxEnemies;
    int activeEnemies;

    public List<HexTile> freeTiles;

    void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        detailMovement = GetComponent<DetailMovement>();

        maxEnemies = 2;
        time = initialSpawningTimer;
    }


    void Update()
    {
        if(gameManager != null && gameManager.hasGameStarted)
        {
            if (activeEnemies != maxEnemies)
                time -= Time.deltaTime;

            if (time <= 0f)
            {
                GrabNeighbours();
                freeTiles.Clear();
                time = timerBetweenEnemies;
            }
        }
    }

    void SpawnEne()
    {
        while (activeEnemies != maxEnemies)
        {
            int randomTile = Random.Range(0, freeTiles.Count);
            int randomDrop = Random.Range(0, enemies.Length);

            GameObject enemy = Instantiate(enemies[randomDrop], freeTiles[randomTile].transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
            enemy.GetComponent<EnemyStateMachine>().currentTile = freeTiles[randomTile];
            enemy.GetComponent<EnemyStateMachine>().ID = "Enemy " + Random.Range(1, 100);
            freeTiles.Remove(freeTiles[randomTile]);
            activeEnemies++;
        }
    }

    void GrabNeighbours()
    {
        foreach (HexTile tile in detailMovement.currentTile.neighbours)
        {
            if (!tile.hasObjects)
            {
                freeTiles.Add(tile);
            }
        }

        SpawnEne();
    }

    public void EnemyKilled()
    {
        activeEnemies--;
    }
}
