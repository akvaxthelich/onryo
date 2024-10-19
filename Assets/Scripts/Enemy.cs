using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int hopeValue;

    public AudioClip deathSound;
    public AudioClip attackSound;
    public Shader flashShader; // in normal projects please put this in a resources folder!
    public Shader flashCritShader;
    protected Shader defaultShader; //currently trying to get the current shader, set it to flash, then replace the original shader
    Shader sh;
    protected SpriteRenderer sr;
    //spriterenderer has a material, that material has a shader. edit that!

    protected LayerMask playerMask;

    protected float offset; // for placing the weapon on either side of the enemy
    protected Transform weapOffsetTransform;
    protected States currState;

    protected float aiTimer; //generic

    protected int health;
    public int maxHealth;
    protected float knockbackAmt = 10;

    protected float movementSpeed;

    protected Rigidbody2D rb;
    protected AudioSource src;
    protected BoxCollider2D bx;
    protected Animator anim; //captain's log: changed all members to protected for direct access via child class,
    //cant believe i fucking forgot the keyword 'protected'


    [SerializeField]
    AudioClip[] clips;

    public virtual void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        currState = States.Idle;
        health = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        sh = sr.material.shader;
        defaultShader = sh;
        anim = GetComponent<Animator>();
        bx = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        src = GetComponent<AudioSource>();
    }

    #region Health, Damage, Death
    public virtual void TakeDamage(int amt, GameObject source) {
        src.PlayOneShot(clips[Random.Range(0,3)]);
        health -= amt;
        KnockbackSelf(knockbackAmt, source.transform);
        StartCoroutine("Flash");

        if (health <= 0)
        {
            Die();
        }

    }

    public virtual void TakeCritDamage(int amt, GameObject source)
    {
        src.PlayOneShot(clips[Random.Range(0, 3)]);
        health -= amt * 2;
        KnockbackSelf(knockbackAmt, source.transform);
        StartCoroutine("CritFlash");

        if (health <= 0)
        {
            Die();
        }

    }

    public void KnockbackSelf(float force, Transform opponent) { 
        Vector2 dir = transform.position - opponent.position;
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    protected virtual void Die() {
        src.PlayOneShot(deathSound);
        StartCoroutine("DeathCoroutine");
        GameData.UpdateHope(hopeValue);
        //fix later for coroutine + anim
    }

    IEnumerator DeathCoroutine() {
        bx.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator Flash() {
        sr.material.shader = flashShader;
        yield return new WaitForSeconds(.15f);
        sr.material.shader = defaultShader;
    }
    IEnumerator CritFlash()
    {
        sr.material.shader = flashCritShader;
        yield return new WaitForSeconds(.15f);
        sr.material.shader = defaultShader;
    }
    #endregion

    #region AI
    public enum States
    { //unused attack states for more complex enemies.
        Idle,
        Wander,
        Attack1,
        Attack2,
        Attack3,
        Dead
    }

    public virtual void TransitionState(States s) {
        StartCoroutine(Transition(s, 0));
    }
    public virtual void TransitionState(States s, float delay) {
        //coroutine. async. - Jojow
        StartCoroutine(Transition(s, delay));
    }
    IEnumerator Transition(States s, float delay) {
        aiTimer = 0;
        yield return new WaitForSeconds(delay);

        currState = s;
        switch (s)
        {
            case States.Idle:
                anim.SetBool("Wander", false);
                anim.SetBool("Idle", true);
                break;
            case States.Wander:
                anim.SetBool("Idle", false);
                anim.SetBool("Wander", true);
                break;
            case States.Attack1:
                anim.SetBool("Wander", false);
                anim.SetBool("Idle", false);
                anim.SetTrigger("Attack"); //attack here once!
                
                break;

            default:

                break;
        }
    }
    public virtual void IdleState() {
        //Do fucking nothing! or set velocity to zero
    }
    public virtual void WanderState() {

    }
    public virtual void Attack1() { 
    
    }
    public virtual void Attack2() { 
    
    }

    public virtual void Attack3() { 
    
    }

    public virtual void Dead() {
    
    }
    #endregion

}
