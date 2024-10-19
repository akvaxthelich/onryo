using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skully : Enemy
{
    protected ParticleSystem ps;
    //if skelton no meat, how come code so meat?
    
    public States stateDebug;
    public float maxWanderTime;
    public float attackDelay = .66f; //try .4s note to self, dont just try random numbers
    public float attackDist = 1.5f;

    public int hopeVal;

    public AudioClip skullySwingSound;
    float dist = 10000;

    PlayerMovement pm;
    Transform playerTransform; //player transgender!!!!!!!!!!!!!! ? transform you idiot! you thought eye was a CONSERVATIVE! yarrdey harrr i am  a poirate
    public override void Start()
    {
        base.Start();
        ps = GetComponentInChildren<ParticleSystem>();
        aiTimer = 0; //for any purpose besides transitions
        movementSpeed = 2;
        knockbackAmt = 1;
        hopeValue = hopeVal;
        attackSound = skullySwingSound;
        weapOffsetTransform = transform.GetChild(0);
        currState = States.Idle;
        pm = GameObject.Find("GKnight").GetComponent<PlayerMovement>();
        offset = .75f;
        playerTransform = pm.gameObject.transform;

        TransitionState(stateDebug, 0); //set to editor specified init state.
    }
    //uhhhhhh madybe somewhere try to make it so there's a knockback state that fucks everything uppppp!
    public override void IdleState() {
        TransitionState(States.Wander, .35f); //stand there like an idiot for a sec
    }
    public override void WanderState() {
        //walk left and right
        if (aiTimer > maxWanderTime) {
            aiTimer = 0;
            movementSpeed = -movementSpeed; //flip
        }
        else {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
        if (dist < attackDist) {
            TransitionState(States.Attack1, .15f);
        }
    }
    public override void Attack1() {
        if (aiTimer >= attackDelay) { //this is so we check when the animation sorta hits on time with the thing
            if (health > 0) {
                src.PlayOneShot(attackSound);
                aiTimer = 0;
                Collider2D col = Physics2D.OverlapBox(new Vector2(weapOffsetTransform.position.x, weapOffsetTransform.position.y), new Vector2(1.5f, 2f), 0, playerMask);
                if (col != null && col.gameObject.CompareTag("Player"))
                {
                    col.gameObject.GetComponent<PlayerMovement>().TakeDamage(this.gameObject);
                }
                else
                {
                    TransitionState(States.Idle, 0);
                }

            }

            //transition to wander .5s after.
        }
    }
    protected override void Die()
    {
        ps.Play();                      //bones everywhere
        TransitionState(States.Idle);   //go to idle, need to interrupt attack as well
        src.PlayOneShot(deathSound);
        weapOffsetTransform.position = new Vector2(0, 10000);
        StartCoroutine("DeathCoroutine");
        GameData.UpdateHope(hopeValue);
        //fix later for coroutine + anim
    }
    //skully doesnt use  attack2/3.
    //red skully does!
    //WIGHT does.
    protected void FaceCorrectDir() {
        if (rb.velocity.x > 0.03f)
        {
            sr.flipX = false;
            weapOffsetTransform.localPosition = new Vector3(offset, 0, 0);
        }
        if (rb.velocity.x < -0.03f)
        {
            weapOffsetTransform.localPosition = new Vector3(-offset, 0, 0);
            sr.flipX = true;
        }
    }
    void Update()
    {
        
        FaceCorrectDir(); //correct facing direction and weapon offset in a handay danday funshin
        aiTimer         += Time.deltaTime; //all purpose timer;
        dist = Vector2.Distance(weapOffsetTransform.position, playerTransform.position); //sazon i mean all purpose seasoning i mean distance!!!!!!!!!!!!!!!!

        switch (currState) {
            case States.Idle:
                IdleState();    //testing.....done.
                break;
            case States.Wander:
                WanderState();  //testing.....done.
                break;
            case States.Attack1:
                Attack1();      //testing.....OK.
                break;
            case States.Dead:
                StartCoroutine("DeathCoroutine");
                                //testing.....done.
                break;
            default:
                break;

        }

    }

}
