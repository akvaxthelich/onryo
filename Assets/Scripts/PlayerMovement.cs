using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    
    public GameObject music;
    //Captain's Note: 1/30/24
    //Grounding works, bug related to wall sticking also fixed.
    //Footsteps not configured, but audio is currently available.
    public Shader flashShader; // in normal projects please put this in a resources folder!
    public Shader flashCritShader;
    protected Shader defaultShader; //currently trying to get the current shader, set it to flash, then replace the original shader
    Shader sh;
    //Horizontal movement
    #region Horizontal Movement Input and Speed
    [SerializeField]
    Vector2 _move;
    [SerializeField]
    float _moveInput;
    [SerializeField]
    public float _moveSpeed = 4.0f;
    [SerializeField]
    //float _attackMovementDampening = 2.0f;
    //how much to dampen movement while attacking. directly subtracted from movement speed
    //precondition: dampening cannot be greater than movement speed!!!
    #endregion

    #region Jumping
    //Jumping
    public float jumpForce = 5f;
    bool jump;
    bool isGrounded;
    #endregion

    #region Attacking
    float cooldownTime = 0.0f;
    [SerializeField]
    float maxCooldown = 2.0f;

    bool attack;
    #endregion

    bool dead = false;

    LayerMask lm;
    LayerMask enemyMask;

    Rigidbody2D rb;
    SpriteRenderer sr;

    SpriteRenderer flailSr;
    float offset = 2;
    Animator anim;

    AudioSource aSrc;
    public AudioClip swingClip;
    public AudioClip jumpClip;
    public AudioClip stepClip;

    public AudioClip deathSound;

    private void Awake()
    {
        lm = LayerMask.GetMask("Ground");
        enemyMask = LayerMask.GetMask("Enemy");
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultShader = sr.material.shader;
        flailSr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        aSrc = GetComponent<AudioSource>();

        isGrounded = false;
        jump = false;
    }
    
    void Update()
    {
        if (!dead)
        {
            _moveInput = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                jump = true;
            }

            if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - .9f), new Vector2(.18f, 0.26f), 0, lm))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            if (_move.x > 0)
            {
                flailSr.gameObject.transform.localPosition = new Vector3(offset, 0, 0);
                anim.SetBool("Run", true);
                sr.flipX = false;
                flailSr.flipX = false;
            }
            else if (_move.x < 0)
            {
                flailSr.gameObject.transform.localPosition = new Vector3(-offset, 0, 0);
                anim.SetBool("Run", true);
                sr.flipX = true;
                flailSr.flipX = true;
            }
            else
            {
                anim.SetBool("Run", false);
                sr.flipX = sr.flipX;
                flailSr.flipX = flailSr.flipX;
            }

            

            cooldownTime += Time.deltaTime;

            if (Input.GetAxis("Fire1") > 0 && (cooldownTime >= maxCooldown))
            {
                cooldownTime = 0;
                anim.SetBool("Attack", true);
                attack = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            _move = new Vector2(_moveInput * _moveSpeed, rb.velocity.y);
            rb.velocity = _move;
            if (jump)
            {
                jump = false;
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                aSrc.PlayOneShot(jumpClip);
            }

            if (attack)
            {
                aSrc.PlayOneShot(swingClip);

                Collider2D col = Physics2D.OverlapBox(new Vector2(flailSr.gameObject.transform.position.x, transform.position.y), new Vector2(2f, 4f), 0, enemyMask);

                if (col != null && col.CompareTag("Enemy"))
                {
                    if (col.GetComponent<SkullJectile>() != null)
                    {
                        col.GetComponent<SkullJectile>().Annihilate();
                    }
                    else if (rb.velocity.y < -0.03)
                    {
                        col.gameObject.GetComponent<Enemy>().TakeCritDamage(1, gameObject);
                    }
                    else
                    {
                        col.gameObject.GetComponent<Enemy>().TakeDamage(1, gameObject);
                    }

                }
                attack = false;
                anim.SetBool("Attack", false);
            }

            if (rb.velocity.y > 0.05f)
            {
                anim.SetBool("Jump", true);
            }
            else
            {
                anim.SetBool("Jump", false);
            }

            if (rb.velocity.y < -0.03f)
            {
                rb.gravityScale = 2;
                anim.SetBool("Fall", true);
            }
            else
            {
                rb.gravityScale = 1;
                anim.SetBool("Fall", false);
            }
        }
        else {

            rb.velocity = new Vector2(0, rb.velocity.y);

        }
    }
    public virtual void TakeDamage(GameObject source)
    {
        Die();
        StartCoroutine("Flash");


    }
    IEnumerator Flash()
    {
        sr.material.shader = flashShader;
        flailSr.material.shader = flashShader;
        yield return new WaitForSeconds(.15f);
        sr.material.shader = defaultShader;
        flailSr.material.shader = defaultShader;
    }

    public void Die() {
        music.SetActive(false);
        tag = "Respawn";
        dead = true;
        GameData.UpdateLives(-1);
        aSrc.PlayOneShot(deathSound);
        anim.SetTrigger("Dead");
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(4.5f);
        Respawn();
    }
    void Respawn() {
        if (GameData.GetLives() <= 0)
        {
            SceneManager.LoadScene(8); //change later for game over scene im lazy blah blah blah spaghetti code everywhere dont look at me please! yardy hard harthe.
        }
        else {
            GameData.ResetHope();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
