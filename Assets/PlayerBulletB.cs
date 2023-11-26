using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletB : MonoBehaviour
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
        if (damage != 0)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyStateMachine>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
