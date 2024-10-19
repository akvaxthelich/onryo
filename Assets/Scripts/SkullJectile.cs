using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullJectile : MonoBehaviour
{
    public float dir = 1; //set when fired
    Rigidbody2D rb;
    ParticleSystem ps;
    SpriteRenderer sr;
    BoxCollider2D bx;
    AudioSource src;
    public AudioClip clip;

    float skulljectileSpeed = 15f;

    private void Start()
    {
        bx = GetComponent<BoxCollider2D>();
        src = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        if (rb.velocity.x > 0.03f)
        {
            sr.flipX = false;
        }
        if (rb.velocity.x < -0.03f)
        {
            sr.flipX = true;
        }
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(dir * skulljectileSpeed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<PlayerMovement>().TakeDamage(gameObject);
            ps.Play();
            Die();
        }
        else {

            Die();
        }
    }

    public void Annihilate() {
        Die();
    }
    void Die() {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine() {

        sr.enabled = false;
        bx.enabled = false;
        src.PlayOneShot(clip);
        yield return new WaitForSeconds(.25f); //quarter second lifetime?  wdym die after .25s?

        Destroy(gameObject);
         //explode into sparks
    }
}
