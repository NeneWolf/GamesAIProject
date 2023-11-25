using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DropBehaviour : MonoBehaviour
{
    
    string tag;
    HexTile tile;
    float halfHeight;
    float hextileY;


    [SerializeField] float speedOfDroppingFromSpawn;

    [SerializeField] int healAmount = 10;
    [SerializeField] int damageIncrease = 2;
    [SerializeField] int increaseMaxHealth = 10;

    SphereCollider sphereCollider;

    private void Awake()
    {
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
            
            Destroy(this.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyStateMachine>().Heal(healAmount);
            Destroy(this.gameObject);
        }

        
    }

    public HexTile ReportTile()
    {
        return tile;
    }
}
