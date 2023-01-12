using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    /*public int maxHealth = 100;
    public int currentHealth;*/
    public CharacterStatus healthBar;
    public float timeBetweenShots = 2f;
    private float timeStamp;

    /* --------------------------------------*/
    private Collider2D coll;
    private float horizontal = 0;
    private float vertical = 0;
    private Animator anim;
    private SpriteRenderer spriRender;

    /* --------------------------------------*/
    private string currentState;

    private bool isJumping;
    private bool isFalling;
    private bool isRunning;
    private bool isGround;
    private bool grounded;
    private bool currentAnimation;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool isDizzy;
    private bool isDeath;
    private bool isCrouched;
    private bool isCrouching;
    private bool isCrouchAttack;
    private bool isHurt;

    [SerializeField] private float attackDelay = 10f;

    const string PLAYER_IDLE = "Player_idle";
    const string PLAYER_RUN = "Player_run";
    const string PLAYER_JUMP = "Player_jump";
    const string PLAYER_ATTACK = "Player_attack";
    const string PLAYER_AIR_ATTACK = "Player_air_attack";
    const string PLAYER_DIZZY = "Player_dizzy";
    const string PLAYER_DEATH = "Player_death";
    const string PLAYER_CROUCH = "Player_crouch";
    const string PLAYER_HURT = "Player_hurt";
    const string PLAYER_CROUCH_ATTACK = "Player_crouch_attack";


    /* --------------------------------------*/

    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public float dirX, dirY;

    //private enum MovementState { idle, running, falling, jumping }

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    public bool facingLeft;

    /* --------------------------------------*/
    void Start()
    {
        /*currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);*/
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriRender = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        if (facingLeft == true)
        {
            facingLeft = false;
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontal * dirX, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            isGround = false;
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAttackPressed = true;
            isAttacking = false;
        }


        /*if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeMedicine(15);
        }*/

        if (Time.time >= timeStamp && (Input.GetKeyDown(KeyCode.E)))
        {
            //Instantiate(bullet, transform.position, transform.rotation);

            timeStamp = Time.time + timeBetweenShots;

            if (isAttackPressed)
            {
                isAttackPressed = false;

                if (!isAttacking)
                {
                    isAttacking = true;
                    if (isGround)
                    {
                        ChangeAnimationState(PLAYER_ATTACK);
                    }
                    else
                    {
                        ChangeAnimationState(PLAYER_AIR_ATTACK);
                    }
                    //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
                    Invoke("AttackComplete", attackDelay);
                    Debug.Log("delay attack works");

                    fireBalls[FindFireBall()].transform.position = firePoint.position;
                    fireBalls[FindFireBall()].GetComponent<BulletController>().SetDirection(Mathf.Sign(transform.localScale.x));
                }
            }
        }

        // Crouch

        if (Input.GetKeyDown(KeyCode.S))
        {
            isCrouching = true;
            //ChangeAnimationState(PLAYER_CROUCH);
            Debug.Log("Crouch Attack");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (isCrouching)
            {
                //isCrouching = false;
                if (isGround)
                {
                    isJumping = false;

                    ChangeAnimationState(PLAYER_CROUCH);
                    Debug.Log("is crouched");
                }

                //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
            }

        }
        //UpdateAnimationState();

        if (GetComponent<CharacterStatus>().currentHealth < 0)
        {
            ChangeAnimationState(PLAYER_HURT);
        }
        else
        {
            if (!isDeath)
            {
                ChangeAnimationState(PLAYER_DEATH);
                gameObject.SetActive(false);
                isDeath = true;
            }
        }
    }

    /*private void UpdateAnimationState()
    {
        MovementState state;

        if (horizontal > 0f)
        {
            state = MovementState.running;
            spriRender.flipX = false;
        }
        else if (horizontal < 0f)
        {
            state = MovementState.running;
            spriRender.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.falling;
        }

        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }*/

    /*private void IsGrounded()
    {
        //isGround = false;
        isGround = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f, groundLayer);
        if (colliders.Length > 0)
        {
            isGround = true;
        }
        
        if (isGround)
        {
            Debug.Log("Ground check work");
        }
    }*/


    private void FixedUpdate()
    {
        //Ground Check
        isGround = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, groundLayer);

        /*if (Input.GetKeyUp(KeyCode.W))
        {
            isCrouching = false;

            anim.GetCurrentAnimatorStateInfo(0);
        }*/

        if (isJumping && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, dirY);
            isJumping = false;
            isGround = false;
            grounded = false;
            ChangeAnimationState(PLAYER_JUMP);

            FallGroundAfterJump();
        }

        void FallGroundAfterJump()
        {
            if (isGround && !isJumping && horizontal == 0f && !isCrouching)
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        //Flip X
        if (facingLeft == true && horizontal > 0f)
        {
            //spriRender.flipX = false;
            Flip();
        }
        else if (facingLeft != true && horizontal < 0f)
        {
            //spriRender.flipX = true;
            Flip();
        }



        // RUN - IDLE
        if (isGround && !isAttacking)
        {
            if (horizontal != 0f)
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else if (horizontal == 0f && !isJumping)
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }

        /*if (isCrouched)
        {
             isCrouched = false;
             if (!isCrouching)
             {
                 isCrouching = true;
                 if (isGround)
                 {
                     ChangeAnimationState(PLAYER_CROUCH);
                     Debug.Log("is crouched");
                 }
                 //attackDelay = anim.GetCurrentAnimatorStateInfo(0).length;
                 Invoke("CrouchComplete", attackDelay);
                
             }
        }*/


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tag")
        {
            grounded = true;
        }
    }

    public bool CanAttackOnGround()
    {
        return horizontal == 0 && isGround && grounded;
    }

    void AttackComplete()
    {
        isAttacking = false;
        Debug.Log("AttackComplete() works");
    }

    void CrouchComplete()
    {
        isCrouching = false;
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentState == newAnimation) return;

        anim.Play(newAnimation);

        currentState = newAnimation;
    }

    /*public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void TakeMedicine(int medicine)
    {
        currentHealth += medicine;
        healthBar.SetHealth(currentHealth);
    }*/

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector2 theScale = new Vector2();
        theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        gameObject.transform.localScale = theScale;
    }

    private int FindFireBall()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    //public GameObject bullet;
}


