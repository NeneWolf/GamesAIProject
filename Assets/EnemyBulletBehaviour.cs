using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    int damage;
    [SerializeField] float speed = 5f;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    private void Start()
    {
        Destroy(this.gameObject, 10f);    
    }

    void Update()
    {
        if(damage!= 0)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Hit");
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Village" || collision.gameObject.tag == "PlayerCastle")
        {
            collision.gameObject.GetComponent<DetailMovement>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
