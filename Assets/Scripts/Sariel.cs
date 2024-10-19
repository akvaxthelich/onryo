using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Sariel : Enemy
{
    protected ParticleSystem ps;
    //if skelton no meat, how come code so meat?
    public States stateDebug;
    public float maxWanderTime;
    public float attackDelay = .4f; //try .4s
    public float attackDist = 1.5f;

    public float jumpForce = 6f;
    protected float jumpTimer = 0;
    float jumpDelay = 1f;

    LayerMask lm;


    bool jump;
    bool isGrounded;

    bool facingDirBool;
    public float facingDirVal;

    public GameObject skulljectile;

    float dist = 10000;

    PlayerMovement pm;
    Transform playerTransform; //player transgender!!!!!!!!!!!!!! ? transform you idiot! you thought eye was a CONSERVATIVE! yarrdey harrr i am  a poirate
    public override void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        aiTimer = 0; //for any purpose besides transitions
        movementSpeed = 2;
        knockbackAmt = 1;
        hopeValue = 17;//hope here
        base.Start();
        weapOffsetTransform = transform.GetChild(0);
        currState = States.Idle;
        pm = GameObject.Find("GKnight").GetComponent<PlayerMovement>();
        offset = .75f;
        lm = LayerMask.GetMask("Ground");
        playerTransform = pm.gameObject.transform;
        facingDirBool = !sr.flipX;
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
    public override void Attack1() { //really more of an aggro state

        if (playerTransform.position.y > transform.position.y + 2 && (jumpTimer >= jumpDelay)) {
            jump = true;
            Jump();
        }
        if (aiTimer >= attackDelay)
        { //this is so we check when the animation sorta hits on time with the thing
            aiTimer = 0;
            if (Physics2D.Raycast(transform.position, facingDirVal * Vector2.right, attackDist, playerMask))
            {
                src.PlayOneShot(attackSound);
                FireSkullJectile();
                TransitionState(States.Wander);
            }

        }
    }

    void FireSkullJectile() {

        GameObject skullject = Instantiate(skulljectile, weapOffsetTransform.position, Quaternion.identity);
        skullject.GetComponent<SkullJectile>().dir = facingDirVal;
            
     }
    protected override void Die()
    { 
        ps.Play();
        TransitionState(States.Idle);
        src.PlayOneShot(deathSound);
        StartCoroutine("DeathCoroutine");
        GameData.UpdateHope(hopeValue);
        //fix later for coroutine + anim
    }
    //SARIEL!

    protected void FaceCorrectDir() {
        if (playerTransform.position.x > transform.position.x)
        {
            sr.flipX = false;
            weapOffsetTransform.localPosition = new Vector3(offset, 0, 0);
            facingDirVal = 1;
        }
        if (playerTransform.position.x < transform.position.x)
        {
            sr.flipX = true;
            weapOffsetTransform.localPosition = new Vector3(-offset, 0, 0);
            facingDirVal = -1;
        }
    }

    void Jump() {
        if (isGrounded && jump) {
            jumpTimer = 0;
            jump = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

    }
    void Update()
    {
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 1f), new Vector2(.18f, 0.5f), 0, lm))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        FaceCorrectDir(); //correct facing direction and weapon offset in a handay danday funshin
        aiTimer         += Time.deltaTime; //all purpose timer;
        jumpTimer       += Time.deltaTime;
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
