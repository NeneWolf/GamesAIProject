using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject parentObject;

    bool hasUpdated = false;

    private void Awake()
    {
        parentObject =  GameObject.FindAnyObjectByType<MapGenerator>().ReturnTileParent();
    }

    public void Start()
    {
        UpdatePosition();            
    }

    public void UpdatePosition()
    {
        this.transform.localScale = new Vector3(5, 5, 5);

        Vector3 parentPosition = parentObject.GetComponent<MeshFilter>().mesh.bounds.extents;

        Vector3 currentGlobalScale = parentObject.transform.lossyScale;

        this.transform.position = transform.position + new Vector3(0, parentPosition.y * currentGlobalScale.y - 0.5f, 0);

    }
}
