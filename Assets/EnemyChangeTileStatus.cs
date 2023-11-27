using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyChangeTileStatus : MonoBehaviour
{
    [SerializeField] GameObject enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<HexTile>().enemy = enemy;
            other.gameObject.GetComponent<HexTile>().hasObjects = true;
            other.gameObject.GetComponent<HexTile>().hasEnemy = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.gameObject.GetComponent<HexTile>().enemy = null;
            other.gameObject.GetComponent<HexTile>().hasObjects = false;
            other.gameObject.GetComponent<HexTile>().hasEnemy = false;
        }
    }
}
