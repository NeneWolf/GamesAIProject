using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    int damage;
    [SerializeField] float speed = 5f;

    [SerializeField] LayerMask whatIsTarget;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    void Update()
    {
        if(damage!= 0)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == whatIsTarget)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage);
            }
            else
            {
                collision.gameObject.GetComponent<DetailMovement>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
