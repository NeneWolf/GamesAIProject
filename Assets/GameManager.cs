using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    TileManager tileManager;
    HexTile tile;

    [SerializeField] GameObject InGameCamera;
    [SerializeField] GameObject InMenuCamera;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject mainMenuCanvas;

    [SerializeField] GameObject diedCanvas;
    [SerializeField] TextMeshProUGUI diedText;

    public bool hasGameStarted;
    bool hasStartedRespawn;

    MapGenerator mapGenerator;

    [SerializeField] GameObject backCanvas;

    private void Awake()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();
        tileManager = FindFirstObjectByType<TileManager>();
    }

    private void Update()
    {
        if(hasGameStarted && !backCanvas.activeInHierarchy) { backCanvas.SetActive(true); }
    }

    public void StartGame(bool hasDecor)
    {
        mapGenerator.spawnDecor = true;
        mapGenerator.GenerateMap();

        StartCoroutine(StartGameCount());
    }

    public void SpawnPlayer()
    {
        tile = tileManager.GetRandomTile();

        while (tile.hasObjects && tile == null && tile.heightWeight > 0.7f)
        {
            tile = tileManager.GetRandomTile();
        }

        RaycastHit hit;

        if (Physics.Raycast(tile.transform.position + new Vector3(0,10, 0), Vector3.down, out hit, 100f) && hit.collider.gameObject.layer == 6)
        {
            GameObject playerInstance = Instantiate(playerPrefab, new Vector3(tile.position.x, hit.point.y, tile.position.z), Quaternion.identity);
            playerInstance.GetComponent<PlayerMovement>().currentTile = tile;
        }
        else
        {
            Debug.Log("FailedToSpawnPlayer");   
            SpawnPlayer();
        }
    }

    void DestroyGhost()
    {
        GameObject ghost = GameObject.FindGameObjectWithTag("GhostPlayer");

        if (ghost != null)
        {
            Destroy(ghost);
        }
    }

    IEnumerator StartGameCount()
    {
        yield return new WaitForSeconds(2f);
        hasGameStarted = true;

        //Destroy GhostPlayer & Reset the tile information
        DestroyGhost();


        //Disable Menu
        mainMenuCanvas.SetActive(false);

        //Switch Cameras
        InGameCamera.SetActive(true);
        InMenuCamera.SetActive(false);

        //In-game UI

        //Spawn Player
        SpawnPlayer();
    }

    public void UIDeadPlayer()
    {
        if (!hasStartedRespawn) {
            StartCoroutine(UIDeadPlayerCoroutine());
        }
        
    }

    IEnumerator UIDeadPlayerCoroutine()
    {
        hasStartedRespawn = true;
        diedCanvas.SetActive(true);
        diedText.text = "You Died!";

        yield return new WaitForSeconds(5f);
        diedText.text = "Respawn in progress...";

        yield return new WaitForSeconds(5f);
        diedText.text = "Player Has respawn!";

        yield return new WaitForSeconds(1f);

        diedCanvas.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
