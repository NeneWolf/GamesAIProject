using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TileManager tileManager;
    HexTile tile;

    [SerializeField] GameObject InGameCamera;
    [SerializeField] GameObject InMenuCamera;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject mainMenuCanvas;

    //Change to private later
    public bool hasDecor;
    private void Awake()
    {
        tileManager = FindFirstObjectByType<TileManager>();
    }

    public void StartGame(bool hasDecor)
    {
        this.hasDecor = hasDecor;

        //Disable Menu
        mainMenuCanvas.SetActive(false);

        //Switch Cameras
        InGameCamera.SetActive(true);
        InMenuCamera.SetActive(false);

        //In-game UI

        //Spawn Player
        SpawnPlayer(playerPrefab, hasDecor);
    }

    void SpawnPlayer(GameObject player, bool hasDecor)
    {
        tile = tileManager.GetRandomTile();

        while (tile.hasObjects)
        {
            tile = tileManager.GetRandomTile();
        }

        RaycastHit hit;

        if (Physics.Raycast(tile.transform.position + new Vector3(0, 50, 0), Vector3.down, out hit, 100f) && hit.collider.gameObject.layer == 6)
        {
            GameObject playerInstance = Instantiate(player, new Vector3(tile.position.x, hit.point.y, tile.position.z), Quaternion.identity);
            playerInstance.GetComponent<PlayerMovement>().currentTile = tile;
        }
    }
}