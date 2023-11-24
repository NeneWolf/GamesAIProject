using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetailMovement : MonoBehaviour
{
    int maxhealth = 100;
    [SerializeField] int currentHealth;

    [SerializeField]
    public GameObject parentObject;
    public HexTile currentTile;

    public bool isDestroyed;
    bool hasUpdated = false;

    private void Awake()
    {
        parentObject = GameObject.FindAnyObjectByType<MapGenerator>().ReturnTileParent();
        currentHealth = maxhealth;
        currentTile = parentObject.GetComponent<HexTile>();
    }

    public void Start()
    {
        UpdatePosition();
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


    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

    }

}
