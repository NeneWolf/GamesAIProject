using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetailMovement : MonoBehaviour
{
    [SerializeField] int health = 100;

    [SerializeField]
    public GameObject parentObject;

    bool hasUpdated = false;

    private void Awake()
    {
        //REMOVE THIS COMMENT LET IT BE ACTIVE JUST COMMENTED FOR TESTING AI
        parentObject =  GameObject.FindAnyObjectByType<MapGenerator>().ReturnTileParent();
    }

    public void Start()
    {
        UpdatePosition();            
    }

    public void UpdatePosition()
    {
        //REMOVE THIS COMMENT LET IT BE ACTIVE JUST COMMENTED FOR TESTING AI
        parentObject.GetComponent<HexTile>().hasObjects = true;

        this.transform.localScale = new Vector3(5, 5, 5);

        Vector3 parentPosition = parentObject.GetComponent<MeshFilter>().mesh.bounds.extents;

        Vector3 currentGlobalScale = parentObject.transform.lossyScale;

        this.transform.position = transform.position + new Vector3(0, parentPosition.y * currentGlobalScale.y - 0.5f, 0);

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
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
}
