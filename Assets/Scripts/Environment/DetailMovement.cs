using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetailMovement : MonoBehaviour
{
    [SerializeField] bool ImportantBuilding;

    GameManager gameManager;
    PathFinder pathFinder;

    int maxhealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField]
    public GameObject parentObject;
    public HexTile currentTile;

    public bool isDestroyed;
    bool hasUpdated;


    [Header("Path Finding for Ghost player")]
    GameObject ghostPlayer;
    public bool displayPath;
    public HexTile target;

    List<HexTile> path;
    List<HexTile> analised = new List<HexTile>();
    List<HexTile> notanalised = new List<HexTile>();

    private void Awake()
    {
        parentObject = GameObject.FindAnyObjectByType<MapGenerator>().ReturnTileParent();
        ghostPlayer = GameObject.FindAnyObjectByType<GhostPlayerB>().gameObject;
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        pathFinder = GameObject.FindAnyObjectByType<PathFinder>();

        target = GameObject.FindAnyObjectByType<GhostPlayerB>().currentTile;

        currentTile = parentObject.GetComponent<HexTile>();
        currentHealth = maxhealth;
        
    }

    public void Start()
    {
        UpdatePosition();
        CheckForPathFinder();
    }

    private void Update()
    {
        if (gameManager.hasGameStarted) { 
            target = GameObject.FindAnyObjectByType<PlayerMovement>().currentTile;

            if (displayPath)
                CheckForPathFinder();
        }
    }

    public void UpdatePosition()
    {
        parentObject.GetComponent<HexTile>().hasObjects = true;

        this.transform.localScale = new Vector3(5, 5, 5);

        Vector3 parentPosition = parentObject.GetComponent<MeshFilter>().mesh.bounds.extents;

        Vector3 currentGlobalScale = parentObject.transform.lossyScale;

        this.transform.position = transform.position + new Vector3(0, parentPosition.y * currentGlobalScale.y - 0.5f, 0);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDestroyed = true;
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        parentObject.GetComponent<HexTile>().hasObjects = false;
        this.GetComponent<BoxCollider>().enabled = false;

        Destroy(this.GetComponent<NavMeshObstacle>());
        GameObject.FindAnyObjectByType<MapGenerator>().GenerateMap();
        Destroy(this.gameObject);
    }

    public bool ReportStatus()
    {
        return isDestroyed;
    }


    void CheckForPathFinder()
    {
        if (!gameManager.hasGameStarted)
            path = PathFinder.FindPath(currentTile, target, true);
        else path = PathFinder.FindPath(currentTile, target, false);

        analised = pathFinder.GetTileListAnalised();
        notanalised = pathFinder.GetTileListNotAnalised();

        if(ImportantBuilding && !gameManager.hasGameStarted)
        {
            foreach (HexTile tile in path)
            {
                if(tile.hasObjects && !tile.isImportantBuilding)
                {
                    DestroyImmediate(tile.DecorInHexigon);
                    tile.hasObjects = false;
                }
            }
        }

    }

    private void OnDrawGizmos()
    {

        if (displayPath)
        {
            foreach (HexTile tile in analised)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(tile.position, 1f);
            }

            foreach (HexTile tile in notanalised)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(tile.position, 1f);
            }

            foreach (HexTile tile in path)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(tile.position, 1f);
            }
        }
    }

}
