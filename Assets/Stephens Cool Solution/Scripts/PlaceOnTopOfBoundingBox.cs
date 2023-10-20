using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnTopOfBoundingBox : MonoBehaviour
{
    /// <summary>
    /// Lore of bounding boxes:
    /// 
    /// Most game engines will create an invisible cube around a mesh to workout how far it reaches. This is usually done to help with things like generating
    /// convex mesh colliders.
    /// 
    /// For a mesh like your hexagons, which have a flat top, we can use this to determine how high up this top is.
    /// 
    /// IMPORTANT: This bounding box will treat your mesh as if it has a scale of (1,1,1).
    /// Scalling your mesh up won't mean the bounding box gets bigger. It's all about the data of the mesh, not what is rendered on screen.
    /// </summary>




    ///This is the gameobject that I want to sit ontop of my hexagon.
    [SerializeField]
    GameObject objectToSitOnTop;

    ///This is a simple Vector2 that sets the possible range of the vertical scale, from -1 to 1 on the Y axis.
    ///I use this to test scaling my hexagon up and down by this amount.
    [SerializeField]
    Vector2 maxScaleChange = new Vector2(-1f,1f);


    ///A boolean to act as a button because I'm lazy and don't like Editor scripting :)
    [SerializeField]
    bool changeScale = false;

    void Update()
    {
        ///Simple null check to stop errors
        if(objectToSitOnTop == null)
            return;


        ///When my button is pressed, the simple transform code scales by hexagon up or down on the y-axis within the range specified by the Vector2 above.
        if (changeScale) 
        {
            transform.localScale = new Vector3(1, Random.Range(maxScaleChange.x, maxScaleChange.y), 1);

            ///Reset my ""button"".
            changeScale = false;
        }

        ///We capture the size of the bounding box as a Vector3 variable.
        Vector3 thisObjectMeshBounds = GetComponent<MeshFilter>().mesh.bounds.extents;

        Vector3 currentGlobalScale = transform.lossyScale;

        ///'bounds.size' means the 'diameter' of the box: The entire box.
        ///'bounds.extents' is an alternative that gives you the 'radius' of the box: it will be half the size of the 'size'.

        ///We then tell our object to match the position of our hexagon, BUT we also use the 'height' of the bounding box multiplied by the global scale of the gameObject to also tell it to move up by the correct amounts.
        objectToSitOnTop.transform.position = transform.position + new Vector3(0, thisObjectMeshBounds.y* currentGlobalScale.y, 0);
    }
}
