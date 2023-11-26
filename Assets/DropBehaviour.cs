using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DropBehaviour : MonoBehaviour
{
    GameManager gameManager;
    PathFinder pathFinder;
    DropsSpawnManager dropsSpawnManager;

    string tag;
    HexTile tile;
    float halfHeight;
    float hextileY;

    [Header("Dropping from Spawn - Information")]
    [SerializeField] float speedOfDroppingFromSpawn;

    [SerializeField] int healAmount = 10;
    [SerializeField] int damageIncrease = 2;
    [SerializeField] int increaseMaxHealth = 10;

    SphereCollider sphereCollider;

    [Header("Path Finding for Ghost player")]
    public bool displayPath;
    public HexTile target;

    List<HexTile> path;
    List<HexTile> analised = new List<HexTile>();
    List<HexTile> notanalised = new List<HexTile>();

    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        pathFinder = GameObject.FindAnyObjectByType<PathFinder>();
        dropsSpawnManager = GameObject.FindAnyObjectByType<DropsSpawnManager>();

        tag = gameObject.tag;
        halfHeight = this.transform.lossyScale.y * 0.5f;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    private void Update()
    {
        float distance = Mathf.Abs(transform.position.y - hextileY);

        if (distance > 0.6f)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100) && hit.collider.gameObject.layer == 6)
            {
                hextileY = hit.point.y;
                tile = hit.collider.gameObject.GetComponent<HexTile>();

                float newY = Mathf.Lerp(transform.position.y, hextileY + halfHeight, Time.deltaTime * speedOfDroppingFromSpawn);

                // Update the position
                transform.position = new Vector3(tile.position.x, newY, tile.position.z);
            }
        }
        else
        {
            sphereCollider.enabled = true;
        }


        if (sphereCollider.enabled && gameManager.hasGameStarted)
        {
            target = GameObject.FindAnyObjectByType<PlayerMovement>().currentTile;

            if (displayPath)
                CheckForPathFinder();
        }
    }

    public int RetrieveValue()
    {
        Debug.Log(tag);

        switch (tag)
        {
            case "Heal":
                return healAmount;
            case "Damage":
                return damageIncrease;
            case "MaxHealth":
                return increaseMaxHealth;
            default: return 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(tag == "Heal")
            {
                other.gameObject.GetComponent<PlayerMovement>().Heal(healAmount);
            }
            else if(tag == "Damage")
            {
                other.gameObject.GetComponent<PlayerMovement>().Increatedamage(damageIncrease);
            }
            else if(tag == "MaxHealth")
            {
                other.gameObject.GetComponent<PlayerMovement>().IncreaseMaxHealth(increaseMaxHealth);
            }

            OnDestroyDrop();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyStateMachine>().Heal(healAmount);
            OnDestroyDrop();
        }
    }

    public HexTile ReportTile()
    {
        return tile;
    }

    void CheckForPathFinder()
    {
        path = PathFinder.FindPath(tile, target, false);

        analised = pathFinder.GetTileListAnalised();
        notanalised = pathFinder.GetTileListNotAnalised();
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

    void OnDestroyDrop()
    {
        dropsSpawnManager.DropDestroyed();
        Destroy(this.gameObject);
    }
}
