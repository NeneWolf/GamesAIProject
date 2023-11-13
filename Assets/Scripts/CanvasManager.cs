using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    MapGenerator mapGenerator;
    GameManager gameManager;

    [SerializeField] TMP_Text currentSeed;
    [SerializeField] Toggle offsetToggle;
    [SerializeField] Toggle spawnDecorToggle;

    bool spawnDecor;

    private void Awake()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    //Seed Id

    public void SeedID(TMP_Text seedText)
    {
        mapGenerator.seed = seedText.text.GetHashCode();
        mapGenerator.GenerateMap();
    }

    public void RandomSeed()
    {
        mapGenerator.seed = Random.Range(0, 500);
        mapGenerator.GenerateMap();
    }

    private void Update()
    {
        currentSeed.text = "Current Seed: " + mapGenerator.seed.ToString();
    }

    public void Offset()
    {
        mapGenerator.useFallOff = offsetToggle.isOn;
        mapGenerator.GenerateMap();
    }

    public void SpawnDecorRandom()
    {
        mapGenerator.spawnDecor = spawnDecorToggle.isOn;
        spawnDecor = spawnDecorToggle.isOn;
        mapGenerator.GenerateMap();
    }

    public void StartGame()
    {
        gameManager.StartGame(spawnDecor);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
