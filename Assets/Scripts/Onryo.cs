using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Onryo : MonoBehaviour
{
    // im sorry
    float lookHowFastIAm = 5.65f;
    PlayerMovement you;
    Rigidbody2D me;
    private void Start()
    {
        me = GetComponent<Rigidbody2D>();
        you = GameObject.Find("GKnight").GetComponent<PlayerMovement>();
        //found you.
    }
    void Kill() { 
        Vector2 thereYouAre = (you.gameObject.transform.position - transform.position).normalized;
        me.velocity = thereYouAre * lookHowFastIAm;
    }

    private void FixedUpdate()
    {
        Kill();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            SceneManager.LoadScene(7);
        }
    }
}
