using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    public void TurnOnOffCollider()
    {
        boxCollider.enabled = !boxCollider.enabled;
    }
}
