using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{

    private void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1000000, gameObject);

        }
        else if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().Die();

        }
        else {
            return;
        }
    }

}
