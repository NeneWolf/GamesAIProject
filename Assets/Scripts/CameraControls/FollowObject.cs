using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public void OnMouseDown()
    {
        CameraController.instance.followTarget = transform;
    }
}
