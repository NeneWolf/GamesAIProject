using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptForEnemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<HexTile>().hasObjects = true;
            collision.gameObject.GetComponent<HexTile>().hasEnemy = true;
            collision.gameObject.GetComponent<HexTile>().enemy = this.gameObject;
        }
        //else if(collision.gameObject.CompareTag("Sword"))
        //{
        //    Debug.Log("Hit");
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<HexTile>().hasObjects = false;
            collision.gameObject.GetComponent<HexTile>().hasEnemy = false;
            collision.gameObject.GetComponent<HexTile>().enemy = null;
        }
    }
}
